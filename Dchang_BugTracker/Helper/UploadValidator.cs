using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace Dchang_BugTracker.Helper
{
    public class UploadValidator
    {
        public static bool IsWebFriendlyImage(HttpPostedFileBase file)
        {
            if (file == null) return false;

            if (file.ContentLength > 2 * 1024 * 1024 || file.ContentLength < 1024) return false;

            try
            {
                using (var img = Image.FromStream(file.InputStream))
                {
                    return ImageFormat.Jpeg.Equals(img.RawFormat) ||
                            ImageFormat.Png.Equals(img.RawFormat) ||
                            ImageFormat.Gif.Equals(img.RawFormat);
                }
            }
            catch
            {
                return false;
            }
        }

        public static bool IsWebFriendlyFile(HttpPostedFileBase file)
        {
            if (file == null) return false;

            //We might need to loosen the size restrictions a bit
            var maxSize = WebConfigurationManager.AppSettings["MaxFileSize"];
            var minSize = WebConfigurationManager.AppSettings["MaxFileSize"];
            if (file.ContentLength > Convert.ToInt32(maxSize) || file.ContentLength < Convert.ToInt32(minSize))
                return false;

            try
            {
                var allowedExtensions = WebConfigurationManager.AppSettings["AllowedExtensions"];
                var fileExt = Path.GetExtension(file.FileName);
                return allowedExtensions.Contains(fileExt);
            }
            catch
            {
                return false;
            }
        }

    }
}