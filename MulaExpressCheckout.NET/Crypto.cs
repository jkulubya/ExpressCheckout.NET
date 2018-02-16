using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace MulaExpressCheckout.NET
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
                aesAlg.KeySize = 256;
                aesAlg.BlockSize = 128;
                aesAlg.Key = Encoding.UTF8.GetBytes(Sha256(_key).Substring(0, 32));
                aesAlg.IV = Encoding.UTF8.GetBytes(Sha256(_iv).Substring(0, 16));
                aesAlg.Padding = PaddingMode.PKCS7;

                var encryptor = aesAlg.CreateEncryptor();

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

            return Convert.ToBase64String(Encoding.UTF8.GetBytes(Convert.ToBase64String(encrypted)));
        }

        public string Decrypt(string cipherText)
        {
            string plaintext = null;
            var cypherBytes = Convert.FromBase64String(Encoding.UTF8.GetString(Convert.FromBase64String(cipherText)));

            using (var aesAlg = Aes.Create())
            {
                aesAlg.Mode = CipherMode.CBC;
                aesAlg.KeySize = 256;
                aesAlg.BlockSize = 128;
                aesAlg.Key = Encoding.UTF8.GetBytes(Sha256(_key).Substring(0, 32));
                aesAlg.IV = Encoding.UTF8.GetBytes(Sha256(_iv).Substring(0, 16));
                aesAlg.Padding = PaddingMode.PKCS7;

                var decryptor = aesAlg.CreateDecryptor();

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

            return plaintext;
        }

        private static string Sha256(string value)
        {
            var sb = new StringBuilder();

            using (var hash = SHA256.Create())            
            {
                var result = hash.ComputeHash(Encoding.UTF8.GetBytes(value));

                foreach (var b in result)
                {
                    sb.Append(b.ToString("x2"));
                }
            }

            return sb.ToString();
        }
    }
}