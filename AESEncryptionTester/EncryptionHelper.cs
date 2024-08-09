﻿using System.Security.Cryptography;

namespace AESEncryptionTester
{
    public static class EncryptionHelper
    {
        private static string _aChars = "abcdefhijklmnopqrstuvwyx";
        private static string _aCharsUp = _aChars.ToUpper();
        private static Random _random = new Random();

        private static char RandomChar
        {
            get
            {
                var allChars = (_aChars + _aCharsUp + " ").ToArray();
                var seed = _random.Next(allChars.Length);
                return allChars[seed];
            }
        }

        /// <summary>
        /// Encrypts a string to bytes using AES algorithm.
        /// </summary>
        /// <param name="plainText">Plaintext (string).</param>
        /// <param name="Key">Symmetric encryption key.</param>
        /// <param name="IV">Intialisation vector.</param>
        /// <returns>Ciphertext (byte array).</returns>
        public static byte[] EncryptStringToBytes_Aes(object? plainText, byte[] Key, byte[] IV)
        {
            // Check arguments.
            if (plainText == null || plainText is string s && s.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");
            byte[] encrypted;

            // Create an Aes object
            // with the specified key and IV.
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create an encryptor to perform the stream transform.
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for encryption.
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            //Write all data to the stream.
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            // Return the encrypted bytes from the memory stream.
            return encrypted;
        }

        /// <summary>
        /// Decrypts ciphertext (byte array) back to corresponding plaintext using the given key.
        /// </summary>
        /// <param name="cipherText">Ciphertext (byte array).</param>
        /// <param name="Key">Symmetric encryption key.</param>
        /// <param name="IV">Initialisation vector.</param>
        /// <returns>Plaintext from ciphertext.</returns>
        public static string DecryptStringFromBytes_Aes(byte[] cipherText, byte[] Key, byte[] IV)
        {
            // Check arguments.
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");

            // Declare the string used to hold
            // the decrypted text.
            string plaintext;

            // Create an Aes object
            // with the specified key and IV.
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create a decryptor to perform the stream transform.
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for decryption.
                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {

                            // Read the decrypted bytes from the decrypting stream
                            // and place them in a string.
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }

            return plaintext;
        }

        /// <summary>
        /// Generates random plaintext string of given length;
        /// </summary>
        /// <param name="length">Length of plaintext to generate (in char).</param>
        /// <returns>Randomly generated string.</returns>
        public static string GenerateRandomPlaintext(int length)
        {
            var plaintext = string.Empty;

            for (int i = 0; i < length; i++)
                plaintext += RandomChar;
            
            return plaintext;
        }

        /// <summary>
        /// Generates a temporary file of a given size (in MB).
        /// </summary>
        /// <param name="length">Size of the file to generation (MB).</param>
        /// <returns>Path to the temp file.</returns>
        public static string GenerateFile(long length)
        {
            var tempFile = Path.GetTempFileName();

            using (var fileStream = new FileStream(tempFile, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                fileStream.SetLength(length * 1024);
            }

            return tempFile;
        }
    }
}
