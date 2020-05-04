using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBase_Generator_WPF
{
    public static class ImageProvider
    {
        static Stream _imageString = Statics.GetStreamFromUrl("https://thispersondoesnotexist.com/image");

        public static byte[] GetImageAsByteArray()
        {
            return ReadToEnd(_imageString);
        }

        public static string GetImageAs64BaseString()
        {
            var image = Base64ToImage(Convert.ToBase64String(ReadToEnd(_imageString)));
            // Bitmap newBitmap = new Bitmap(i);
            var imageFormat = image.GetImageFormat(out string extension);
            image.Save("sampleImage." + extension, imageFormat);
            return Convert.ToBase64String(ReadToEnd(_imageString));
        }

        private static byte[] ReadToEnd(System.IO.Stream stream)
        {
            long originalPosition = 0;

            if (stream.CanSeek)
            {
                originalPosition = stream.Position;
                stream.Position = 0;
            }

            try
            {
                byte[] readBuffer = new byte[4096];

                int totalBytesRead = 0;
                int bytesRead;

                while ((bytesRead = stream.Read(readBuffer, totalBytesRead, readBuffer.Length - totalBytesRead)) > 0)
                {
                    totalBytesRead += bytesRead;

                    if (totalBytesRead == readBuffer.Length)
                    {
                        int nextByte = stream.ReadByte();
                        if (nextByte != -1)
                        {
                            byte[] temp = new byte[readBuffer.Length * 2];
                            Buffer.BlockCopy(readBuffer, 0, temp, 0, readBuffer.Length);
                            Buffer.SetByte(temp, totalBytesRead, (byte)nextByte);
                            readBuffer = temp;
                            totalBytesRead++;
                        }
                    }
                }

                byte[] buffer = readBuffer;
                if (readBuffer.Length != totalBytesRead)
                {
                    buffer = new byte[totalBytesRead];
                    Buffer.BlockCopy(readBuffer, 0, buffer, 0, totalBytesRead);
                }
                return buffer;
            }
            finally
            {
                if (stream.CanSeek)
                {
                    stream.Position = originalPosition;
                }
            }
        }

        private static Image Base64ToImage(string base64String)
        {
            // Convert base 64 string to byte[]
            byte[] imageBytes = Convert.FromBase64String(base64String);
            // Convert byte[] to Image
            using (var ms = new MemoryStream(imageBytes, 0, imageBytes.Length))
            {
                Image image = Image.FromStream(ms, true);
                return image;                
            }
        }

        private static System.Drawing.Imaging.ImageFormat GetImageFormat(this System.Drawing.Image img, out string imageExtension)
        {          
            if (img.RawFormat.Equals(System.Drawing.Imaging.ImageFormat.Jpeg))
            {
                imageExtension = "jpg";
                return System.Drawing.Imaging.ImageFormat.Jpeg;
            }
            if (img.RawFormat.Equals(System.Drawing.Imaging.ImageFormat.Bmp))
            {
                imageExtension = "bmp";
                return System.Drawing.Imaging.ImageFormat.Bmp;
            }
            if (img.RawFormat.Equals(System.Drawing.Imaging.ImageFormat.Png))
            {
                imageExtension = "png";
                return System.Drawing.Imaging.ImageFormat.Png;
            }
            if (img.RawFormat.Equals(System.Drawing.Imaging.ImageFormat.Emf))
            {
                imageExtension = "emf";
                return System.Drawing.Imaging.ImageFormat.Emf;
            }
            if (img.RawFormat.Equals(System.Drawing.Imaging.ImageFormat.Exif))
            {
                imageExtension = "exif";
                return System.Drawing.Imaging.ImageFormat.Exif;
            }
            if (img.RawFormat.Equals(System.Drawing.Imaging.ImageFormat.Gif))
            {
                imageExtension = "gif";
                return System.Drawing.Imaging.ImageFormat.Gif;
            }
            if (img.RawFormat.Equals(System.Drawing.Imaging.ImageFormat.Icon))
            {
                imageExtension = "gif";
                return System.Drawing.Imaging.ImageFormat.Icon;
            }
            if (img.RawFormat.Equals(System.Drawing.Imaging.ImageFormat.MemoryBmp))
            {
                imageExtension = "memoryBmp";
                return System.Drawing.Imaging.ImageFormat.MemoryBmp;
            }
            if (img.RawFormat.Equals(System.Drawing.Imaging.ImageFormat.Tiff))
            {
                imageExtension = "tiff";
                return System.Drawing.Imaging.ImageFormat.Tiff;
            }
            else
            {
                imageExtension = "wmf";
                return System.Drawing.Imaging.ImageFormat.Wmf;
            }
        }
    }
}
