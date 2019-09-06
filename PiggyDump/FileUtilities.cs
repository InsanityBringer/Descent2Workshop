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

namespace PiggyDump
{
    public class FileUtilities
    {
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
    }
}
