using Flight_Center_Project_FinalExam_DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Flight_Center_Project_FinalExam_testingInterface
{
    static class Statics
    {
        public static string UniqeRandomString(int smallest, int biggest)
        {
            smallest--;
            List<int> possibilities = new List<int>();
            for (int i = smallest; i <= biggest; i++) possibilities.Add(i);

            string output = string.Empty;

            Random rnd = new Random();

            int rangeCount = biggest - smallest;

            for (int i = 0; i < rangeCount; i++)
            {
                int randomIndex = rnd.Next(0, possibilities.Count() - 1) + 1;
                output += possibilities[randomIndex];
                possibilities.Remove(possibilities[randomIndex]);
            }
            return output;
        }



        public static void drawBorder<T>(this T drawableObject, int borderWidth, Color bordercolor) where T : class
        {
            int width = 0; int height = 0;
            if (drawableObject is Image) { width = (drawableObject as Image).Width; height = (drawableObject as Image).Height; }
            if (drawableObject is Control) { width = (drawableObject as Control).Width; height = (drawableObject as Control).Height; }

            Bitmap bitmap = new Bitmap(width, height);
            Graphics graphicsObj = Graphics.FromImage(bitmap);

            Pen myPen = new Pen(bordercolor, borderWidth);
            graphicsObj.DrawRectangle(myPen, 0, 0, width - 1, height - 1);

            if (drawableObject is Image) drawableObject = bitmap as T;
            else
            {
                drawableObject.GetType().GetProperty("BackgroundImage")?.SetValue(drawableObject, bitmap);
                drawableObject.GetType().GetProperty("Image")?.SetValue(drawableObject, bitmap);
            }
            graphicsObj.Dispose();
        }



        static public string GetUniqueKeyOriginal_BIASED(int size)
        {
            char[] chars =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray();
            byte[] data = new byte[size];
            using (RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider())
            {
                crypto.GetBytes(data);
            }
            StringBuilder result = new StringBuilder(size);
            foreach (byte b in data)
            {
                result.Append(chars[b % (chars.Length)]);
            }
            return result.ToString();
        }

        static public string FirstLetterToupper(this string str)
        {
            string strOut = string.Empty;
            for(int i = 0; i < str.Length; i++)
            {
                if (i == 0) strOut += Char.ToUpper(str[i]);
                else strOut += str[i];
            }
            return strOut;
        }








        // This constant is used to determine the keysize of the encryption algorithm in bits.
        // We divide this by 8 within the code below to get the equivalent number of bytes.
        private const int Keysize = 256;

        // This constant determines the number of iterations for the password bytes generation function.
        private const int DerivationIterations = 1000;

        public static string Encrypt(string plainText, string passPhrase)
        {
            // Salt and IV is randomly generated each time, but is preprended to encrypted cipher text
            // so that the same Salt and IV values can be used when decrypting.  
            var saltStringBytes = Generate256BitsOfRandomEntropy();
            var ivStringBytes = Generate256BitsOfRandomEntropy();
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            using (var password = new Rfc2898DeriveBytes(passPhrase, saltStringBytes, DerivationIterations))
            {
                var keyBytes = password.GetBytes(Keysize / 8);
                using (var symmetricKey = new RijndaelManaged())
                {
                    symmetricKey.BlockSize = 256;
                    symmetricKey.Mode = CipherMode.CBC;
                    symmetricKey.Padding = PaddingMode.PKCS7;
                    using (var encryptor = symmetricKey.CreateEncryptor(keyBytes, ivStringBytes))
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                            {
                                cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                                cryptoStream.FlushFinalBlock();
                                // Create the final bytes as a concatenation of the random salt bytes, the random iv bytes and the cipher bytes.
                                var cipherTextBytes = saltStringBytes;
                                cipherTextBytes = cipherTextBytes.Concat(ivStringBytes).ToArray();
                                cipherTextBytes = cipherTextBytes.Concat(memoryStream.ToArray()).ToArray();
                                memoryStream.Close();
                                cryptoStream.Close();
                                return Convert.ToBase64String(cipherTextBytes);
                            }
                        }
                    }
                }
            }
        }

        public static string Decrypt(string cipherText, string passPhrase)
        {
            // Get the complete stream of bytes that represent:
            // [32 bytes of Salt] + [32 bytes of IV] + [n bytes of CipherText]
            var cipherTextBytesWithSaltAndIv = Convert.FromBase64String(cipherText);
            // Get the saltbytes by extracting the first 32 bytes from the supplied cipherText bytes.
            var saltStringBytes = cipherTextBytesWithSaltAndIv.Take(Keysize / 8).ToArray();
            // Get the IV bytes by extracting the next 32 bytes from the supplied cipherText bytes.
            var ivStringBytes = cipherTextBytesWithSaltAndIv.Skip(Keysize / 8).Take(Keysize / 8).ToArray();
            // Get the actual cipher text bytes by removing the first 64 bytes from the cipherText string.
            var cipherTextBytes = cipherTextBytesWithSaltAndIv.Skip((Keysize / 8) * 2).Take(cipherTextBytesWithSaltAndIv.Length - ((Keysize / 8) * 2)).ToArray();

            using (var password = new Rfc2898DeriveBytes(passPhrase, saltStringBytes, DerivationIterations))
            {
                var keyBytes = password.GetBytes(Keysize / 8);
                using (var symmetricKey = new RijndaelManaged())
                {
                    symmetricKey.BlockSize = 256;
                    symmetricKey.Mode = CipherMode.CBC;
                    symmetricKey.Padding = PaddingMode.PKCS7;
                    using (var decryptor = symmetricKey.CreateDecryptor(keyBytes, ivStringBytes))
                    {
                        using (var memoryStream = new MemoryStream(cipherTextBytes))
                        {
                            using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                            {
                                var plainTextBytes = new byte[cipherTextBytes.Length];
                                var decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
                                memoryStream.Close();
                                cryptoStream.Close();
                                return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
                            }
                        }
                    }
                }
            }
        }

        private static byte[] Generate256BitsOfRandomEntropy()
        {
            var randomBytes = new byte[32]; // 32 Bytes will give us 256 bits.
            using (var rngCsp = new RNGCryptoServiceProvider())
            {
                // Fill the array with cryptographically secure random bytes.
                rngCsp.GetBytes(randomBytes);
            }
            return randomBytes;
        }

        public static Bitmap ResizeImageProportionally(Bitmap bitmap, int newWidth)
        {
            int hewHeight = newWidth / (bitmap.Width / bitmap.Height);

            return new Bitmap(bitmap, newWidth, hewHeight);
        }


        public static void WriteMessages(this PictureBox pictureBox, string message1, int fontSize1, string message2, int fontSize2)
        {
            Bitmap messagesBtmp = new Bitmap(pictureBox.Width, pictureBox.Height);
            Graphics graphicsObj = Graphics.FromImage(messagesBtmp);
            Font drawFont = new Font("Arial", fontSize1);
            SolidBrush drawBrush = new SolidBrush(Color.Red);
            graphicsObj.DrawString(message1, drawFont, drawBrush, 10, 20);
            drawFont = new Font("Arial", fontSize2, FontStyle.Bold);

            drawBrush = new SolidBrush(Color.DarkBlue);
            graphicsObj.DrawString(message2, drawFont, drawBrush, 50, 90);
            graphicsObj.Dispose();
            pictureBox.Image = messagesBtmp;
        }
        public static void WriteNumberMessages(this PictureBox picturebox, int hitPoints, string message)
        {
            Bitmap hitPointsBtmp = new Bitmap(picturebox.Width, picturebox.Height);
            Graphics graphicsObj = Graphics.FromImage(hitPointsBtmp);

            Brush myBrush = new SolidBrush(Color.LightBlue);
            graphicsObj.FillRectangle(myBrush, 0, 0, hitPointsBtmp.Width, hitPointsBtmp.Height);
            Font drawFont = new Font("Arial", 36);
            SolidBrush drawBrush = new SolidBrush(Color.White);
            graphicsObj.DrawString(hitPoints.ToString(), drawFont, drawBrush, 10, 20);
            drawFont = new Font("Arial", 12, FontStyle.Bold);

            graphicsObj.DrawString(message, drawFont, drawBrush, 0, 3);

            graphicsObj.Dispose();
            picturebox.Image = hitPointsBtmp;
        }


        public static void ShowTheMethodNames<T>(this DAO<T> dao) where T : class, IPoco, new()
        {
            string str = string.Empty;
            int count = 0;
            foreach (var s in dao.GetType().GetMethods())
            {
                str += dao.GetType().GetMethods()[count].Name + "\n";
                count++;
            }
            MessageBox.Show(str);
        }
        static public string ChopCharsFromTheEnd(this string str, int charsNum)
        {
            string strOut = string.Empty;
            for (int i = 0; i < str.Length; i++)
            {
                if (i < str.Length - charsNum) strOut += str[i];
            }
            return strOut;
        }
        static public string ChopCharsFromTheEndInverted(this string str, int charsNum)
        {
            return ChopCharsFromTheEnd(str, str.Length - charsNum);
        }           
        public static string  Pluralize(this string pocoTypeName)
        {
            string str = string.Empty;
            if (pocoTypeName.Last().Equals('y')) str = pocoTypeName.ChopCharsFromTheEnd(1) + "ies";
            else str = pocoTypeName + "s";

            return str;
        }
        public static string AllSqlTadaTypes()
        {
            string str = string.Empty;
            var dataTypes = Array.ConvertAll((SqlDbType[])Enum.GetValues(typeof(SqlDbType)), type => type.ToString());
            foreach (var s in dataTypes) str += s + "\n";
            return str;
        }


    }



}
