using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Wikiled.Instagram.Api.Helpers
{
    public static class Encrypt
    {
        // This size of the IV (in bytes) must = (keysize / 8).  Default keysize is 256, so the IV must be
        // 32 bytes long.  Using a 16 character string here gives us 32 bytes when converted to a byte array.
        private const string initVector = "pemgail1agkqzl88";

        // This constant is used to determine the keysize of the encryption algorithm
        private const int keysize = 256;

        private const string salt = "this is my random salt. Very complicated";

        public static string EncryptString(string plainText, string passPhrase)
        {
            var initVectorBytes = Encoding.UTF8.GetBytes(initVector);
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            byte[] saltArray = Encoding.ASCII.GetBytes(salt);
            var password = new Rfc2898DeriveBytes(passPhrase, saltArray);
            var keyBytes = password.GetBytes(keysize / 8);
            var symmetricKey = new RijndaelManaged();
            symmetricKey.Mode = CipherMode.CBC;
            var encryptor = symmetricKey.CreateEncryptor(keyBytes, initVectorBytes);
            byte[] cipherTextBytes;
            using (var memoryStream = new MemoryStream())
            {
                using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                {
                    cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                    cryptoStream.FlushFinalBlock();
                    cipherTextBytes = memoryStream.ToArray();
                    memoryStream.Close();
                    cryptoStream.Close();
                }
            }

            return Convert.ToBase64String(cipherTextBytes);
        }

        public static string DecryptString(string cipherText, string passPhrase)
        {
            var initVectorBytes = Encoding.UTF8.GetBytes(initVector);
            var cipherTextBytes = Convert.FromBase64String(cipherText);
            byte[] saltArray = Encoding.ASCII.GetBytes(salt);
            var password = new Rfc2898DeriveBytes(passPhrase, saltArray);
            var keyBytes = password.GetBytes(keysize / 8);
            var symmetricKey = new RijndaelManaged();
            symmetricKey.Mode = CipherMode.CBC;
            var decryptor = symmetricKey.CreateDecryptor(keyBytes, initVectorBytes);
            int decryptedByteCount;
            byte[] plainTextBytes;
            using (var memoryStream = new MemoryStream(cipherTextBytes))
            {
                using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                {
                    plainTextBytes = new byte[cipherTextBytes.Length];
                    decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
                    memoryStream.Close();
                    cryptoStream.Close();
                }
            }

            return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
        }
    }
}
