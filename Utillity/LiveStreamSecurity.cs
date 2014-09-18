using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Text;
using System.Security.Cryptography;

namespace News
{
    public class LiveStreamSecurity
    {
        public static string GenerateToken(string InputString, string key="")
        {
            string ResultString = "";

            //1.Encrypted string with base64
            //2.replace encrypted string(1)  '+' with '–', '/' with '_' and '=' with ','
            //3.split encrypted string(2) length 5 character
            //4.reorder string(3) from before end group to first group and end group
            //5.Encrypted 'key' with base64 ten concat with string(4) then encrypted result with base64
            //6.replace encrypted string(5)  '+' with '–', '/' with '_' and '=' with ','

            //----------

            //1.Encrypted string with base64
            string EncryptedString = "";
            byte[] bytes = Encoding.UTF8.GetBytes(InputString);
            EncryptedString = Convert.ToBase64String(bytes);

            //2.replace encrypted string(1)  '+' with '–', '/' with '_' and '=' with ','
            EncryptedString = EncryptedString.Replace("+","-");
            EncryptedString = EncryptedString.Replace("/", "_");
            EncryptedString = EncryptedString.Replace("=", ",");

            //3.split encrypted string(2) length 5 character
            int ArraySize = (int)Math.Ceiling((double)(EncryptedString.Length) / 5.0);
            string[] EncryptedStringArray = new string[ArraySize];
            for (int i = 0; i < ArraySize; i++)
            {
                int StringLenght = 0;
                if (EncryptedString.Length >= 5)
                {
                    StringLenght = 5;
                }
                else
                {
                    StringLenght = EncryptedString.Length;
                }
                EncryptedStringArray[i] = EncryptedString.Substring(0, StringLenght);
                EncryptedString = EncryptedString.Substring(StringLenght, EncryptedString.Length - StringLenght);
            }

            //4.reorder string(3) from before end group to first group and end group
            string[] EncryptedStringArrayShuffled = new string[ArraySize];
            EncryptedStringArrayShuffled[ArraySize - 1] = EncryptedStringArray[ArraySize - 1];
            for (int i = 0; i < ArraySize-1; i++)
            {
                EncryptedStringArrayShuffled[i] = EncryptedStringArray[ArraySize - 2 - i];
            }
            string EncryptedStringShuffled = "";
            for (int i = 0; i < ArraySize; i++)
            {
                EncryptedStringShuffled += EncryptedStringArrayShuffled[i];
            }

            //5.Encrypted 'key' with base64 ten concat with string(4) then encrypted result with base64
            byte[] bytes_key = Encoding.UTF8.GetBytes(key);
            string encrypted_key = Convert.ToBase64String(bytes_key);
            encrypted_key += EncryptedStringShuffled;
            byte[] bytes_2 = Encoding.UTF8.GetBytes(encrypted_key);
            ResultString = Convert.ToBase64String(bytes_2);
            
            //6.replace encrypted string(5)  '+' with '–', '/' with '_' and '=' with ','
            ResultString = ResultString.Replace("+", "-");
            ResultString = ResultString.Replace("/", "_");
            ResultString = ResultString.Replace("=", ",");

            return ResultString;
        }

        public static string GenerateSecureKey(string InputString)
        {
            //1.encrypt with MD5
            string hex_MD5 = MD5Core.GetHashString(InputString);
            hex_MD5 = hex_MD5.ToLower();

            //2.encrypt with SHA1
            SHA1 sha = new SHA1Managed();
            byte[] input_data_SHA1 = Encoding.UTF8.GetBytes(hex_MD5);
            byte[] encrypt_result_SHA1 = sha.ComputeHash(input_data_SHA1);
            string hex_with_spliter_SHA1 = BitConverter.ToString(encrypt_result_SHA1);
            string hex_SHA1 = hex_with_spliter_SHA1.Replace("-", "");
            hex_SHA1 = hex_SHA1.ToLower();

            return hex_SHA1;
        }

        public static string GenerateTransactionID(long Milliseconds)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            origin = origin.AddMilliseconds(Milliseconds);
            return origin.Year.ToString() + origin.Month.ToString("00") + origin.Day.ToString("00") + origin.Hour.ToString("00") + origin.Minute.ToString("00") + origin.Second.ToString("00") + GenerateDigit(6);
        }

        private static String GenerateDigit(int maxLength)
        {
            String pass = "";
            String pwd = "";
            pwd += "00112233445566778899";

            try
            {
                char[] pwdChars = pwd.ToCharArray();
                pwdChars = ShuffleCharacter(pwdChars);
                Random rand = new Random();
                for (int i = 0; i < maxLength; i++)
                {
                    pass += pwdChars[rand.Next(pwdChars.Length)];
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }

            return pass;
        }

        private static char[] ShuffleCharacter(char[] characters)
        {
            Random rand = new Random();
            for (int j = 0; j < characters.Length; j++)
            {
                int from = rand.Next(characters.Length);
                char old = characters[j];
                characters[j] = characters[from];
                characters[from] = old;
            }

            return characters;
        }
        /*
        public static long ConvertDateTimeToUnixTimestamp(DateTime date)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            TimeSpan diff = date - origin;
            return (long)Math.Floor(diff.TotalSeconds);
        }

        public static string ConvertUnixTimestampToStringDateTime(long Milliseconds)
        {
            DateTime origin = new DateTime(1970, 1, 1, 7, 0, 0, 0);
            origin = origin.AddMilliseconds(Milliseconds);
            return origin.Year.ToString() + origin.Month.ToString("00") + origin.Day.ToString("00") + origin.Hour.ToString("00") + origin.Minute.ToString("00");
        }
        */
    }
}
