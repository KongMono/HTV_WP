using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace MovieTV
{
    public class AES
    {
        /// <summary>
        /// Encrypt a string using AES
        /// </summary>
        /// <param name="Str">String to encrypt</param>
        /// <param name="Password">Encryption password</param>
        /// <param name="Salt">A salt string (at least 8 bytes)</param>
        /// <returns>Encrypted string in case of success; otherwise - empty string</returns>
        public static string EncryptString(string Str, string Password = "tdcm1234", string Salt = "tdcm1234")
        {
            try
            {
                using (Aes aes = new AesManaged())
                {
                    Rfc2898DeriveBytes deriveBytes = new Rfc2898DeriveBytes(Password, Encoding.UTF8.GetBytes(Salt));
                    aes.Key = deriveBytes.GetBytes(128 / 8);
                    aes.IV = aes.Key;
                    using (MemoryStream encryptionStream = new MemoryStream())
                    {
                        using (CryptoStream encrypt = new CryptoStream(encryptionStream, aes.CreateEncryptor(), CryptoStreamMode.Write))
                        {
                            byte[] utfD1 = UTF8Encoding.UTF8.GetBytes(Str);
                            encrypt.Write(utfD1, 0, utfD1.Length);
                            encrypt.FlushFinalBlock();
                        }
                        return Convert.ToBase64String(encryptionStream.ToArray());
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error : AES ~ EncryptString ; " + ex.Message);
                return "";
            }
        }

        /// <summary>
        /// Decrypt encrypted string
        /// </summary>
        /// <param name="Str">Encrypted string</param>
        /// <param name="Password">Password used for encryption</param>
        /// <param name="Salt">Salt string used for encryption (at least 8 bytes)</param>
        /// <returns>Decrypted string if success; otherwise - empty string</returns>
        public static string DecryptString(string Str, string Password = "tdcm1234", string Salt = "tdcm1234")
        {
            try
            {
                using (Aes aes = new AesManaged())
                {
                    Rfc2898DeriveBytes deriveBytes = new Rfc2898DeriveBytes(Password, Encoding.UTF8.GetBytes(Salt));
                    aes.Key = deriveBytes.GetBytes(128 / 8);
                    aes.IV = aes.Key;

                    using (MemoryStream decryptionStream = new MemoryStream())
                    {
                        using (CryptoStream decrypt = new CryptoStream(decryptionStream, aes.CreateDecryptor(), CryptoStreamMode.Write))
                        {
                            byte[] encryptedData = Convert.FromBase64String(Str);
                            decrypt.Write(encryptedData, 0, encryptedData.Length);
                            decrypt.Flush();
                        }
                        byte[] decryptedData = decryptionStream.ToArray();
                        return UTF8Encoding.UTF8.GetString(decryptedData, 0, decryptedData.Length);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error : AES ~ DecryptString ; " + ex.Message);
                return "";
            }
        }
    }
}
