using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Configuration;

namespace BugTracker.Helpers
{
    public static class HelperMethods
    {
        public static bool EnsureDirectoryExists(string dirPath)
        {
            bool ret = false;
            if (!String.IsNullOrWhiteSpace(dirPath))
            {
                if (Directory.Exists(dirPath))
                {
                    ret = true;
                }
                else
                {
                    try
                    {
                        Directory.CreateDirectory(dirPath);
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine(e);
                    }

                    Thread.Sleep(1000);
                    ret = Directory.Exists(dirPath);
                }
            }
            return ret;
        }

        /// <summary>
        /// Check if file upload is within configured size and file extension
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static bool IsWebFriendlyFile(HttpPostedFileBase file)
        {
            if (file == null)
            {
                return false;
            }

            bool ret = true;

            // default to 11534336
            int maxSizeBytes = 11 * 1024 * 1024;
            if (Int32.TryParse(WebConfigurationManager.AppSettings["MaxFileUploadSize"], out var maxSize))
            {
                maxSizeBytes = maxSize;
            }

            if (file.ContentLength > maxSizeBytes || file.ContentLength < 1024)
            {
                ret = false;
            }
            try
            {
                var allowableFileExtensions = ".7z,.doc,.docx,.json,.msg,.pdf,.xls,.xlsx,.odf,.odt,.txt,.zip";
                var configuredAllowableFileExtensions = WebConfigurationManager.AppSettings["AllowableFileExtensions"];
                if (!String.IsNullOrWhiteSpace(configuredAllowableFileExtensions))
                {
                    allowableFileExtensions = configuredAllowableFileExtensions;
                }

                string[] allowableFileExtensionsArr = allowableFileExtensions.Split(',');
                var fileExt = Path.GetExtension(file.FileName);
                if (fileExt == null)
                {
                    ret = false;
                }
                else
                {
                    if (!allowableFileExtensionsArr.Contains(fileExt.ToLower()))
                    {
                        ret = false;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                ret = false;
            }

            return ret;
        }

        /// <summary>
        /// Check if image is within configured size and image format
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static bool IsWebFriendlyImage(HttpPostedFileBase file)
        {
            if (file == null)
            {
                return false;
            }

            bool ret = true;

            // Set a default of 3145728
            int maxSizeBytes = 3 * 1024 * 1024;
            if (Int32.TryParse(WebConfigurationManager.AppSettings["MaxImageUploadSize"], out var maxSize))
            {
                maxSizeBytes = maxSize;
            }

            if (file.ContentLength > maxSizeBytes || file.ContentLength < 1024)
            {
                ret = false;
            }
            try
            {
                using (var img = Image.FromStream(file.InputStream))
                {
                    ret = ImageFormat.Jpeg.Equals(img.RawFormat) ||
                          ImageFormat.Png.Equals(img.RawFormat) ||
                          ImageFormat.Gif.Equals(img.RawFormat);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                ret = false;
            }

            return ret;
        }
    }
}
