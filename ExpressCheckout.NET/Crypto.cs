using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace ExpressCheckout.NET
{
    public class Crypto
    {
        private readonly string _iv;
        private readonly string _key;

        public Crypto(string iv, string key)
        {
            if (iv == null || iv.Length <= 0)
            {
                throw new ArgumentNullException(nameof(iv));
            }

            if (key == null || key.Length <= 0)
            {
                throw new ArgumentNullException(nameof(key));
            }
                
            _iv = iv;
            _key = key;
        }
        
        public string Encrypt(string plainText)
        {
            byte[] encrypted;
            using (var aesAlg = Aes.Create())
            {
                aesAlg.Mode = CipherMode.CBC;
                aesAlg.KeySize = 128;
                aesAlg.BlockSize = 128;
                aesAlg.Key = Encoding.UTF8.GetBytes(_key);
                aesAlg.IV = Encoding.UTF8.GetBytes(_iv.Substring(0,16));
                aesAlg.Padding = PaddingMode.Zeros;

                var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (var msEncrypt = new MemoryStream())
                {
                    using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (var swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            return Bytes2Hex(encrypted);
        }

        public string Decrypt(string cipherText)
        {
            string plaintext = null;
            var cypherBytes = Hex2Bytes(cipherText);

            using (var aesAlg = Aes.Create())
            {
                aesAlg.Mode = CipherMode.CBC;
                aesAlg.KeySize = 128;
                aesAlg.BlockSize = 128;
                aesAlg.Key = Encoding.UTF8.GetBytes(_key);
                aesAlg.IV = Encoding.UTF8.GetBytes(_iv.Substring(0,16));
                aesAlg.Padding = PaddingMode.Zeros;

                var decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (var msDecrypt = new MemoryStream(cypherBytes))
                {
                    using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (var srDecrypt = new StreamReader(csDecrypt))
                        {
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }

            }

            return DropTrailingNullPadding(plaintext);
        }

        private static string Bytes2Hex(byte[] input)
        {
            var hex = new StringBuilder(input.Length * 2);
            foreach (var b in input)
            {
                hex.AppendFormat("{0:x2}", b);
            }
                
            return hex.ToString();
        }

        private static byte[] Hex2Bytes(string input)
        {
            var numberChars = input.Length;
            var bytes = new byte[numberChars / 2];
            for (var i = 0; i < numberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(input.Substring(i, 2), 16);
            return bytes;
        }
        
        private static string DropTrailingNullPadding(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return text;
            
            var charLocation = text.IndexOf('\0');
            return charLocation > 0 ? text.Substring(0, charLocation) : text;
        }

    }
}