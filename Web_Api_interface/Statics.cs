using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Dynamic;
using System.Net;
using Flight_Center_Project_FinalExam_BL;
using System.Security.Claims;
using Flight_Center_Project_FinalExam_DAL;

namespace Web_Api_interface
{
    //enumerartion for setting up the method
    //"GetUniqueKeyOriginal_BIASED"
    public enum Charset
    {
        OnlyNumber,
        OnlyUpperCaseLetters,
        OnlyLowerCaseLetters,
        AllAviliableChars
    }

    static class Statics
    {
        private static Random _rnd = new Random();
        


        public static List<DataType> ShuffleList<DataType>(List<DataType> inputList)
        {
            List<DataType> randomList = new List<DataType>();

            Random r = new Random();
            int randomIndex = 0;
            while (inputList.Count > 0)
            {
                randomIndex = r.Next(0, inputList.Count - 1); //Choose a random object in the list
                randomList.Add(inputList[randomIndex]); //add it to the new, random list
                inputList.RemoveAt(randomIndex); //remove to avoid duplicates
            }

            return randomList; //return the new random list
        }

        public static string ReadFromUrl(string url)
        {
            string read_str = "";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

            // read data
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                read_str = reader.ReadToEnd();
            }
            return read_str;
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
        static public string GetUniqueKeyOriginal_BIASED(int size)
        {
            return GetUniqueKeyOriginal_BIASED_Internal(size, Charset.AllAviliableChars);
        }
        static public string GetUniqueKeyOriginal_BIASED(int size, Charset charset)
        {
            return GetUniqueKeyOriginal_BIASED_Internal(size, charset);
        }
        static private string GetUniqueKeyOriginal_BIASED_Internal(int size, Charset charset)
        {
            char[] chars = null;

            if (charset == Charset.AllAviliableChars) chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray();
            if (charset == Charset.OnlyLowerCaseLetters) chars = "abcdefghijklmnopqrstuvwxyz".ToCharArray();
            if (charset == Charset.OnlyUpperCaseLetters) chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
            if (charset == Charset.OnlyNumber) chars = "1234567890".ToCharArray();

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

        static public string DashingString(string input, int num)
        {
            int globalCount = 0;
            int dashingCount = 0;
            string output = string.Empty;
            int count = 0;
            foreach(var s in input)
            {
                count++;
                globalCount++;
                output += s;
                if(count == num)
                {
                    dashingCount++;
                    if(globalCount != input.Length) output += "-";
                    count = 0;
                    
                }
            }
            return output;
        }

        public static Stream GetStreamFromUrl(string url)
        {
            byte[] imageData = null;

            using (var wc = new System.Net.WebClient())
                imageData = wc.DownloadData(url);

            return new MemoryStream(imageData);
        }

        public static DateTime GetRandomDate(DateTime from, DateTime to)
        {
            TimeSpan range = new TimeSpan(to.Ticks - from.Ticks);
            return from + new TimeSpan((long)(range.Ticks * _rnd.NextDouble()));
        }








        /// <summary>
        /// This method compares a two Poco objects of the same type by all the properties except ID
        /// </summary>
        /// <typeparam name="T">type of the objects</typeparam>
        /// <param name="o1">The first compared object</param>
        /// <param name="o2">The second compared object</param>
        /// <returns></returns>
        public static bool BulletprofComparsion<T>(T o1, T o2) where T : IPoco
        {
            bool isEqual = true;
            for (int i = 1; i < typeof(T).GetProperties().Length; i++)
            {
                var value1 = typeof(T).GetProperties()[i].GetValue(o1);
                var value2 = typeof(T).GetProperties()[i].GetValue(o2);

                if (!value1.Equals(value2))
                {
                    isEqual = false;
                    break;
                }
            }
            return isEqual;
        }

    }





}
