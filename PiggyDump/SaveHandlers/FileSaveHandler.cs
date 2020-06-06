/*
    Copyright (c) 2020 SaladBadger

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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Descent2Workshop.SaveHandlers
{
    public class FileSaveHandler : SaveHandler
    {
        private readonly string filename;
        private readonly string newfilename, bakfilename;

        private string statusMsg = "";

        private Stream stream;

        public FileSaveHandler(string filename)
        {
            this.filename = filename;

            //Since filename can't mutate, these can be computed early on
            this.newfilename = filename + ".new";
            this.bakfilename = filename + ".bak";
        }

        public override void Destroy()
        {
        }

        public override bool FinalizeStream()
        {
            if (stream != null)
            {
                stream.Close();
                stream.Dispose();
            }

            //And now the fun task of shuffling around the nonsense. 
            try
            {
                File.Delete(bakfilename);
            }
            catch (FileNotFoundException) { }
            catch (DirectoryNotFoundException) { } //Discover this with our face to avoid a 1/1000000 race condition
            catch (UnauthorizedAccessException exc)
            {
                statusMsg = string.Format("Cannot delete old backup file {0}: Permission denied.\r\nMsg: {1}\r\n", bakfilename, exc.Message);
                return true;
            }
            catch (IOException exc)
            {
                statusMsg = string.Format("Cannot delete old backup file {0}: IO error occurred.\r\nMsg: {1}\r\n", bakfilename, exc.Message);
                return true;
            }
            //Move the current file into the backup slot
            try
            {
                File.Move(filename, bakfilename);
            }
            catch (FileNotFoundException) { }
            catch (DirectoryNotFoundException) { } //Discover this with our face to avoid a 1/1000000 race condition
            catch (UnauthorizedAccessException exc)
            {
                statusMsg = string.Format("Cannot move old file {0}: Permission denied.\r\nMsg: {1}\r\n", filename, exc.Message);
                return true;
            }
            catch (IOException exc)
            {
                statusMsg = string.Format("Cannot move old file {0}: IO error occurred.\r\nMsg: {1}\r\n", filename, exc.Message);
                return true;
            }

            //Move the new file into the current slot
            try
            {
                File.Move(newfilename, filename);
            }
            catch (FileNotFoundException) { }
            catch (DirectoryNotFoundException) { } //Discover this with our face to avoid a 1/1000000 race condition
            catch (UnauthorizedAccessException exc)
            {
                statusMsg = string.Format("Cannot move new file {0}: Permission denied.\r\nMsg: {1}\r\n", newfilename, exc.Message);
                return true;
            }
            catch (IOException exc)
            {
                statusMsg = string.Format("Cannot move new file {0}: IO error occurred.\r\nMsg: {1}\r\n", newfilename, exc.Message);
                return true;
            }

            return false;
        }

        public override Stream GetStream()
        {
            Stream stream = null;
            try
            {
                stream = File.Open(newfilename, FileMode.Create);
            }
            catch (FileNotFoundException)
            {
                statusMsg = string.Format("Error saving data file {0}:\r\nFile not found.", filename);
            }
            catch (PathTooLongException)
            {
                statusMsg = string.Format("Error saving data file {0}:\r\nPath specified is too long.", filename);
            }
            catch (DirectoryNotFoundException)
            {
                statusMsg = string.Format("Error saving data file {0}:\r\nDirectory not found. ", filename);
            }
            catch (UnauthorizedAccessException)
            {
                statusMsg = string.Format("Error saving data file {0}:\r\nPermission denied.", filename);
            }
            catch (InvalidDataException exc)
            {
                statusMsg = string.Format("Error saving data file {0}:\r\n{1}", filename, exc.Message);
            }
            catch (Exception exc)
            {
                statusMsg = string.Format("Unexpected error saving data file {0}:\r\n{1}", filename, exc.Message);
            }

            return stream;
        }

        public override string GetUIName()
        {
            return filename;
        }

        public override string GetErrorMsg()
        {
            return statusMsg;
        }
    }
}
