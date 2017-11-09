using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;

namespace WebLayer.Healper
{
    public static class ImageWorker
    {
        public static ImageFormat GetImageFormat(string imageFormat)
        {
            if (imageFormat.Equals(".png"))
                return ImageFormat.Png;
            else if (imageFormat.Equals(".gif"))
                return ImageFormat.Gif;
            else
                return ImageFormat.Jpeg;
        }

        public static string GetImageType(string base64)
        {
            string imageHeader = Path.GetExtension(base64);//base64.Substring(0, 20);
            if (imageHeader.Contains("png"))
            {
                return ".png";
            }
            else if (imageHeader.Contains("gif"))
            {
                return ".gif";
            }
            else
            {
                return ".jpg";
            }
        }
        public static byte[] GetImageBytesFromBase64(string base64)
        {
            string imageString = base64.Substring(base64.IndexOf("base64") + 7);
            int len = imageString.Length % 4;
            if (len > 0) imageString = imageString.PadRight(imageString.Length + (4 - len), '=');
            string converted = imageString.Replace('-', '+');
            converted = converted.Replace('_', '/');
            return Convert.FromBase64String(imageString);
        }
        public static void SaveImageFromBytes(byte[] bytes, string dir, string fileName, int width, string imageType)
        {
            try
            {
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);
                fileName = width + "_" + fileName;

                using (var ms = new MemoryStream(bytes, 0, bytes.Length))
                {
                    using (Bitmap tempImage = new Bitmap(Image.FromStream(ms)))
                    {
                        int w = tempImage.Width;
                        int h = tempImage.Height;
                        if (w > width)
                        {
                            h = width * h / w;
                            w = width;
                        }
                        new Bitmap(tempImage, w, h).Save(Path.Combine(dir, fileName), GetImageFormat(imageType));
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static Bitmap SaveImageFromBytesTry(byte[] bytes, int width)
        {
            try
            {
                using (var ms = new MemoryStream(bytes, 0, bytes.Length))
                {
                    using (Bitmap tempImage = new Bitmap(Image.FromStream(ms)))
                    {
                        int w = tempImage.Width;
                        int h = tempImage.Height;
                        if (w > width)
                        {
                            h = width * h / w;
                            w = width;
                        }
                        return new Bitmap(tempImage, w, h);
                    }
                }

            }
            catch (Exception)
            {
                return null;
            }
        }

        public static void DeleteFile(string file)
        {
            if (File.Exists(file))
                File.Delete(file);
        }
    }
}