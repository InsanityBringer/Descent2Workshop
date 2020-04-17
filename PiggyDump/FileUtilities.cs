/*
    Copyright (c) 2019 SaladBadger

    Permission is hereby granted, free of charge, to any person obtaining a copy
    of this software and associated documentation files (the "Software"), to deal
    in the Software without restriction, including without limitation the rights
    to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
    copies of the Software, and to permit persons to whom the Software is
    furnished to do so, subject to the following conditions:

    The above copyright notice and this permission notice shall be included in all
    copies or substantial portions of the Software.

    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
    AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
    LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
    OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
    SOFTWARE.
*/

using System;
using System.IO;
using System.Windows.Forms; //TODO: this is a bit too intimate to winforms but it'll do for now. 
using LibDescent.Data;

namespace Descent2Workshop
{
    public class FileUtilities
    {
        public static bool LoadDataFile(string filename, IDataFile dataFile, out string statusMsg)
        {
            bool success = true;
            statusMsg = "";
            Stream stream = null;
            try
            {
                stream = File.OpenRead(filename);
                dataFile.Read(stream);
            }
            catch (FileNotFoundException)
            {
                statusMsg = string.Format("Error opening data file {0}:\r\nFile not found.", filename);
                success = false;
            }
            catch (PathTooLongException)
            {
                statusMsg = string.Format("Error opening data file {0}:\r\nPath specified is too long.", filename);
                success = false;
            }
            catch (DirectoryNotFoundException)
            {
                statusMsg = string.Format("Error opening data file {0}:\r\nDirectory not found. ", filename);
                success = false;
            }
            catch (UnauthorizedAccessException)
            {
                statusMsg = string.Format("Error opening data file {0}:\r\nPermission denied.", filename);
                success = false;
            }
            catch (InvalidDataException exc)
            {
                statusMsg = string.Format("Error reading data file {0}:\r\n{1}", filename, exc.Message);
                success = false;
            }
            catch (Exception exc)
            {
                statusMsg = string.Format("Unexpected error reading data file {0}:\r\n{1}", filename, exc.Message);
                success = false;
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }
            return success;
        }

        public static bool SaveDataFile(string filename, IDataFile dataFile, out string statusMsg)
        {
            bool success = true;
            statusMsg = "";
            string workingFilename = Path.ChangeExtension(filename, "new");
            string backupFilename = filename + ".bak";

            //Write the temp file
            Stream stream = null;
            try
            {
                stream = File.OpenWrite(workingFilename);
                dataFile.Write(stream);
            }
            catch (FileNotFoundException)
            {
                statusMsg = string.Format("Error saving data file {0}:\r\nFile not found.", filename);
                success = false;
            }
            catch (PathTooLongException)
            {
                statusMsg = string.Format("Error saving data file {0}:\r\nPath specified is too long.", filename);
                success = false;
            }
            catch (DirectoryNotFoundException)
            {
                statusMsg = string.Format("Error saving data file {0}:\r\nDirectory not found. ", filename);
                success = false;
            }
            catch (UnauthorizedAccessException)
            {
                statusMsg = string.Format("Error saving data file {0}:\r\nPermission denied.", filename);
                success = false;
            }
            catch (InvalidDataException exc)
            {
                statusMsg = string.Format("Error saving data file {0}:\r\n{1}", filename, exc.Message);
                success = false;
            }
            catch (Exception exc)
            {
                statusMsg = string.Format("Unexpected error saving data file {0}:\r\n{1}", filename, exc.Message);
                success = false;
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }
            if (!success) return success;

            //Doing backup
            try
            {
                File.Delete(backupFilename);
            }
            catch (FileNotFoundException) { } //Discover this with our face to avoid a 1/1000000 race condition
            catch (DirectoryNotFoundException) { } //these are common and shouldn't cause the fail condition
            catch (UnauthorizedAccessException exc)
            {
                statusMsg = string.Format("Cannot delete old backup file {0}:\r\nPermission denied.", backupFilename);
                success = false;
            }
            catch (IOException exc)
            {
                statusMsg = string.Format("Cannot delete old backup file {0}:\r\nIO error occurred.", backupFilename);
                success = false;
            }
            if (!success) return success; //Can potentially recover but eh something's fishy already

            try
            {
                File.Move(filename, backupFilename);
            }
            catch (FileNotFoundException) { }
            catch (DirectoryNotFoundException) { } //Discover this with our face to avoid a 1/1000000 race condition
            catch (UnauthorizedAccessException exc)
            {
                statusMsg = string.Format("Cannot move old data file {0}:\r\nPermission denied.", filename);
            }
            catch (IOException exc)
            {
                statusMsg = string.Format("Cannot move old data file {0}:\r\nIO error occurred.\r\n", filename);
            }
            if (!success) return success; //Well this is fatal, can't rewrite the old file. 

            try
            {
                File.Move(workingFilename, filename);
            }
            catch (FileNotFoundException) { }
            catch (DirectoryNotFoundException) { } //Discover this with our face to avoid a 1/1000000 race condition
            catch (UnauthorizedAccessException exc)
            {
                statusMsg = string.Format("Cannot move new data file {0}:\r\nPermission denied.", filename);
            }
            catch (IOException exc)
            {
                statusMsg = string.Format("Cannot move new data file {0}:\r\nIO error occurred.\r\n", filename);
            }
            if (!success) return success; //Well this is fatal, can't rewrite the old file. 

            return success;
        }

        public static int GetErrorCode(Exception error)
        {
            if (error is FileNotFoundException)
            {
                return -3;
            }
            else if (error is UnauthorizedAccessException)
            {
                return -4;
            }
            else
            {
                return -5;
            }
        }

        public static string FileErrorCodeHandler(int code, string accessType, string fileType)
        {
            if (code == -1)
                return string.Format("{0} has invalid signature and may be corrupt, or of the wrong format.\r\n", fileType);
            else if (code == -2)
                return string.Format("{0} is unknown version.\r\n", fileType);
            else if (code == -3)
                return string.Format("The specified {0} was not found.\r\n", fileType);
            else if (code == -4)
                return string.Format("You do not have permission to {0} the specified {1}.\r\n", accessType, fileType);
            else
                return string.Format("Unknown error trying to {0} {1}.\r\n", accessType, fileType);
        }

        public static string FileExceptionHandler(Exception error, string context)
        {
            if (error is FileNotFoundException)
            {
                return String.Format("The specified {0} was not found.\r\n", context);
            }
            else if (error is UnauthorizedAccessException)
            {
                return String.Format("You do not have permission to access the specified {0}.\r\n", context);
            }
            else
            {
                return String.Format("Unhandled error loading {0}: {1}.\r\n", context, error.Message);
            }
        }
    }
}
