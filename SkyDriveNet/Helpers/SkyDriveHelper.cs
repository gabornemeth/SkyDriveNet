using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkyDriveNet.Helpers
{
    public class SkyDriveHelper
    {
        public static bool IsRootPath(string path)
        {
            return string.IsNullOrEmpty(path) || path == "/";
        }

        public static string GetFolderId(string folderId)
        {
            return IsRootPath(folderId) ? "me/skydrive" : folderId;
        }


    }
}
