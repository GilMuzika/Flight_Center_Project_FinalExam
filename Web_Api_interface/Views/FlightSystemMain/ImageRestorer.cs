using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;

namespace Web_Api_interface.Views.FlightSystemMain
{
    static public class ImageRestorer
    {


        static public string GetFormattedImage64baseString(string unformattedBase64String)
        {
            ImageFormat format = GetBitmapFrom64baseString(unformattedBase64String).GetImageFormat(out string extension);

            string formatAsString = format.ToString();
            
            return String.Format("data:image/"+ extension +";base64,{0}", unformattedBase64String);
        }

        public static Bitmap GetBitmapFrom64baseString(string unformattedBase64String)
        {
            byte[] byteBuffer = Convert.FromBase64String(unformattedBase64String);
            MemoryStream memoryStream = new MemoryStream(byteBuffer);

            memoryStream.Position = 0;

            Bitmap bmp = (Bitmap)Bitmap.FromStream(memoryStream);

            memoryStream.Close();

            return bmp;
        }

        private static System.Drawing.Imaging.ImageFormat GetImageFormat(this System.Drawing.Image img, out string extension)
        {
            if (img.RawFormat.Equals(System.Drawing.Imaging.ImageFormat.Jpeg))
            {
                extension = "jpeg";
                return System.Drawing.Imaging.ImageFormat.Jpeg;
            }
            if (img.RawFormat.Equals(System.Drawing.Imaging.ImageFormat.Bmp))
            {
                extension = "bmp";
                return System.Drawing.Imaging.ImageFormat.Bmp;
            }
            if (img.RawFormat.Equals(System.Drawing.Imaging.ImageFormat.Png))
            {
                extension = "png";
                return System.Drawing.Imaging.ImageFormat.Png;
            }
            if (img.RawFormat.Equals(System.Drawing.Imaging.ImageFormat.Emf))
            {
                extension = "emf";
                return System.Drawing.Imaging.ImageFormat.Emf;
            }
            if (img.RawFormat.Equals(System.Drawing.Imaging.ImageFormat.Exif))
            {
                extension = "exif";
                return System.Drawing.Imaging.ImageFormat.Exif;
            }
            if (img.RawFormat.Equals(System.Drawing.Imaging.ImageFormat.Gif))
            {
                extension = "gif";
                return System.Drawing.Imaging.ImageFormat.Gif;
            }
            if (img.RawFormat.Equals(System.Drawing.Imaging.ImageFormat.Icon))
            {
                extension = "ico";
                return System.Drawing.Imaging.ImageFormat.Icon;
            }
            if (img.RawFormat.Equals(System.Drawing.Imaging.ImageFormat.MemoryBmp))
            {
                extension = "memoryBmp";
                return System.Drawing.Imaging.ImageFormat.MemoryBmp;
            }
            if (img.RawFormat.Equals(System.Drawing.Imaging.ImageFormat.Tiff))
            {
                extension = "tif";
                return System.Drawing.Imaging.ImageFormat.Tiff;
            }
            else
            {
                extension = "wmf";
                return System.Drawing.Imaging.ImageFormat.Wmf;
            }
        }



        private static byte[] ReadStreamToEnd(System.IO.Stream stream)
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






    }
}