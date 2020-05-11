using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flight_Center_Project_FinalExam_DAL
{
    public static class EncryptionProvider
    {
        //encripting phrase for encripting and decripting usernames and passwords

        const string ENCRIPTION_PHRASE = "4r8rjfnklefjkljghggGKJHnif5r5242";
        //static string ENCRIPTION_PHRASE = ConfigurationManager.AppSettings["Encription_Phrase"];

        public static string Encrypt(string plainText)
        {
            return Statics.Encrypt(plainText, ENCRIPTION_PHRASE);
        }

        public static string Decryprt(string cypherText)
        {
            return Statics.Decrypt(cypherText, ENCRIPTION_PHRASE);
        }
    }
}
