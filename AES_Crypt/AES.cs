using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace AES_Crypt
{
    internal class AES
    {
        private static readonly byte[] Key = Encoding.UTF8.GetBytes("0123456789ABCDEF"); // 16-byte key
        private static readonly byte[] IV = Encoding.UTF8.GetBytes("1234567890ABCDEF"); // 16-byte initialization vector

        public static string Encrypt(string plainText)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = Key;
                aes.IV = IV;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                byte[] encryptedBytes = null;

                using (var ms = new System.IO.MemoryStream())
                {
                    using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        using (var sw = new System.IO.StreamWriter(cs))
                        {
                            sw.Write(plainText);
                        }

                        encryptedBytes = ms.ToArray();
                    }
                }

                return Convert.ToBase64String(encryptedBytes);
            }
        }

        public static string Decrypt(string encryptedText)
        {
            byte[] cipherTextBytes = Convert.FromBase64String(encryptedText);

            using (Aes aes = Aes.Create())
            {
                aes.Key = Key;
                aes.IV = IV;

                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                string decryptedText = null;

                using (var ms = new System.IO.MemoryStream(cipherTextBytes))
                {
                    using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                    {
                        using (var sr = new System.IO.StreamReader(cs))
                        {
                            decryptedText = sr.ReadToEnd();
                        }
                    }
                }

                return decryptedText;
            }
        }
    }
}
