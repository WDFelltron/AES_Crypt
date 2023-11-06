using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AES_Crypt
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private string EncryptText(string plainText)
        {
            using (Aes aes = Aes.Create())
            {
                byte[] key = Encoding.UTF8.GetBytes("YourEncryptionKey");
                byte[] iv = Encoding.UTF8.GetBytes("YourEncryptionIV");

                aes.Key = key;
                aes.IV = iv;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                byte[] encryptedBytes;
                using (var ms = new System.IO.MemoryStream())
                {
                    using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
                        cs.Write(plainBytes, 0, plainBytes.Length);
                    }
                    encryptedBytes = ms.ToArray();
                }

                string encryptedText = Convert.ToBase64String(encryptedBytes);
                return encryptedText;
            }
        }

        private string DecryptText(string encryptedText)
        {
            using (Aes aes = Aes.Create())
            {
                byte[] key = Encoding.UTF8.GetBytes("YourEncryptionKey");
                byte[] iv = Encoding.UTF8.GetBytes("YourEncryptionIV");

                aes.Key = key;
                aes.IV = iv;

                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                byte[] encryptedBytes = Convert.FromBase64String(encryptedText);
                byte[] decryptedBytes;
                using (var ms = new System.IO.MemoryStream(encryptedBytes))
                {
                    using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                    {
                        using (var sr = new System.IO.StreamReader(cs))
                        {
                            string decryptedText = sr.ReadToEnd();
                            return decryptedText;
                        }
                    }
                }
            }
        }

        private void Encrypt_Click(object sender, RoutedEventArgs e)
        {
            string plainText = txtPlainText.Text;
            string encryptedText = EncryptText(plainText);
            txtEncryptedText.Text = encryptedText;
        }

        private void Decrypt_Click(object sender, RoutedEventArgs e)
        {
            string encryptedText = txtEncryptedText.Text;
            string decryptedText = DecryptText(encryptedText);
            txtDecryptedText.Text = decryptedText;
        }
    }
}