using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
