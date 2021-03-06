﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Drawing;
using System.IO;
using System.Threading;

namespace IrasBlog.Helpers
{
    public static class HelperMethods
    {
        public static bool IsWebFriendlyImage(HttpPostedFileBase file)
        {
            if (file == null)
            {
                return false;
            }

            if (file.ContentLength > 2 * 1024 * 1024 || file.ContentLength < 1024)
            {
                return false;
            }
            try
            {
                using (var img = Image.FromStream(file.InputStream))
                {
                    return ImageFormat.Jpeg.Equals(img.RawFormat) ||
                           ImageFormat.Png.Equals(img.RawFormat) ||
                           ImageFormat.Gif.Equals(img.RawFormat);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return false;
            }
        }

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
    }
}
