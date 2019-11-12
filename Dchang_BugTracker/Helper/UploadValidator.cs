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

        public static string GetIcon(string fileName) 
        {
            var imgPath = "";
            var ext = Path.GetExtension(fileName);
            switch (ext) 
            {
                case ".pdf":
                    imgPath = "/Images/icons/pdf.png";
                    break;
                case ".doc":
                    imgPath = "/Images/icons/doc.png";
                    break;
                case ".docx":
                    imgPath = "/Images/icons/docx.png";
                    break;
                case ".xls":
                    imgPath = "/Images/icons/xls.png";
                    break;
                case ".xlsx":
                    imgPath = "/Images/icons/xlsx.png";
                    break;
                case ".txt":
                    imgPath = "/Images/icons/txt.png";
                    break;
                case ".zip":
                case ".rar":
                case ".7z":
                    imgPath = "/Images/icons/zip.png";
                    break;
                case ".xml":
                    imgPath = "/Images/icons/xml.png";
                    break;
                case ".jpg":
                case ".gif":
                case ".png":
                    imgPath = fileName;
                    break;
                default:
                    imgPath = "/Images/icons/blank.png";
                    break;
            }
            return imgPath;
        }





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
            var minSize = WebConfigurationManager.AppSettings["MinFileSize"];
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