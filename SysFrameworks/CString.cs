using System;
using System.Collections.Generic;
using System.Text;

namespace SysFrameworks
{
    public class CString
    {
        public static string GetRandomPassword()
        {
            String s_SpecialChar = "!@#$%QWERTYUIOPASDFGHJKLZXCVBNM";
            String s_Digit = "0987654321";
            String s_Nomal = "qwertyuiopasdfghjklzxcvbnm";
            String rndString = "";
            while (true)
            {
                bool isSpecial = false;
                bool isDigit = false;
                Random r = new Random();
                for (int i = 1; i <= Constant.C_MIN_PASSWORD_LENGTH; ++i)
                {                    
                    int choise = r.Next(1, 3);
                    if (choise == 1)
                    {
                        //Lấy ký tự đặc biệt
                        rndString = rndString + s_SpecialChar.Substring(r.Next(0, s_SpecialChar.Length-1), 1);
                        isSpecial = true;
                    }
                    else if (choise == 2)
                    {
                        //Lấy ký tự số
                        rndString = rndString + s_Digit.Substring(r.Next(0, s_Digit.Length - 1), 1);
                        isDigit = true;
                    }
                    else
                    {
                        rndString = rndString + s_Nomal.Substring(r.Next(0, s_Nomal.Length - 1), 1);
                        //Lấy ký tự thường
                    }
                }
                if (isSpecial && isDigit)
                {
                    break;
                }
            }
            return rndString;
        }
        public static bool IsValidEmail(string email)
        {
            var trimmedEmail = email.Trim();

            if (trimmedEmail.EndsWith("."))
            {
                return false; // suggested by @TK-421
            }
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == trimmedEmail;
            }
            catch
            {
                return false;
            }
        }
        public static string EncodePasswordToBase64(string password)
        {
            try
            {
                byte[] encData_byte = new byte[password.Length];
                encData_byte = System.Text.Encoding.UTF8.GetBytes(password);
                string encodedData = Convert.ToBase64String(encData_byte);
                return encodedData;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in base64Encode" + ex.Message);
            }
        }

        public static string DecodeFrom64(string encodedData)
        {
            System.Text.UTF8Encoding encoder = new System.Text.UTF8Encoding();
            System.Text.Decoder utf8Decode = encoder.GetDecoder();
            byte[] todecode_byte = Convert.FromBase64String(encodedData);
            int charCount = utf8Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length);
            char[] decoded_char = new char[charCount];
            utf8Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);
            string result = new String(decoded_char);
            return result;
        }

        public static string GetRandomDigit(int numDigit)
        {
            String s_Digit = "0987654321";
            string rndString = "";
            Random r = new Random();
            for (int i = 1; i <= numDigit; ++i)
            {                                
                rndString = rndString + s_Digit.Substring(r.Next(0, s_Digit.Length - 1), 1); 
            }
            return rndString;
        }

        public static string RemoveUnicode(string text)
        {
            string[] arr1 = new string[] { "á", "à", "ả", "ã", "ạ", "â", "ấ", "ầ", "ẩ", "ẫ", "ậ", "ă", "ắ", "ằ", "ẳ", "ẵ", "ặ",
                "đ",
                "é","è","ẻ","ẽ","ẹ","ê","ế","ề","ể","ễ","ệ",
                "í","ì","ỉ","ĩ","ị",
                "ó","ò","ỏ","õ","ọ","ô","ố","ồ","ổ","ỗ","ộ","ơ","ớ","ờ","ở","ỡ","ợ",
                "ú","ù","ủ","ũ","ụ","ư","ứ","ừ","ử","ữ","ự",
                "ý","ỳ","ỷ","ỹ","ỵ",};
            string[] arr2 = new string[] { "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a",
                "d",
                "e","e","e","e","e","e","e","e","e","e","e",
                "i","i","i","i","i",
                "o","o","o","o","o","o","o","o","o","o","o","o","o","o","o","o","o",
                "u","u","u","u","u","u","u","u","u","u","u",
                "y","y","y","y","y",};
            for (int i = 0; i < arr1.Length; i++)
            {
                text = text.Replace(arr1[i], arr2[i]);
                text = text.Replace(arr1[i].ToUpper(), arr2[i].ToUpper());
            }
            return text;
        }
    }
}
