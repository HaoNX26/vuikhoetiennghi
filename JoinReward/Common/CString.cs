using System;
using System.Text;
using System.Text.RegularExpressions;

namespace JoinReward.Common
{
    public class CString
    {
        public static string RemoveSpecialCharacters(string str)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in str)
            {
                if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || c == '.' || c == '_')
                {
                    sb.Append(c);
                }
            }
            if (string.IsNullOrEmpty(sb.ToString()))
                return "default";
            return sb.ToString();
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

        public static string RemoveUnicodeFile(string text)
        {
            string[] arr1 = new string[] { "á", "à", "ả", "ã", "ạ", "â", "ấ", "ầ", "ẩ", "ẫ", "ậ", "ă", "ắ", "ằ", "ẳ", "ẵ", "ặ",
                "đ",
                "é","è","ẻ","ẽ","ẹ","ê","ế","ề","ể","ễ","ệ",
                "í","ì","ỉ","ĩ","ị",
                "ó","ò","ỏ","õ","ọ","ô","ố","ồ","ổ","ỗ","ộ","ơ","ớ","ờ","ở","ỡ","ợ",
                "ú","ù","ủ","ũ","ụ","ư","ứ","ừ","ử","ữ","ự",
                "ý","ỳ","ỷ","ỹ","ỵ"," "};
            string[] arr2 = new string[] { "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a",
                "d",
                "e","e","e","e","e","e","e","e","e","e","e",
                "i","i","i","i","i",
                "o","o","o","o","o","o","o","o","o","o","o","o","o","o","o","o","o",
                "u","u","u","u","u","u","u","u","u","u","u",
                "y","y","y","y","y",""};
            for (int i = 0; i < arr1.Length; i++)
            {
                text = text.Replace(arr1[i], arr2[i]);
                text = text.Replace(arr1[i].ToUpper(), arr2[i].ToUpper());
            }
            return text;
        }
        //public static string GetRandomPassword()
        //{
        //    String s_SpecialChar = "!@#$%QWERTYUIOPASDFGHJKLZXCVBNM";
        //    String s_Digit = "0987654321";
        //    String s_Nomal = "qwertyuiopasdfghjklzxcvbnm";
        //    String rndString = "";
        //    while (true)
        //    {
        //        bool isSpecial = false;
        //        bool isDigit = false;
        //        Random r = new Random();
        //        for (int i = 1; i <= Constant.C_MIN_PASSWORD_LENGTH; ++i)
        //        {
        //            int choise = r.Next(1, 3);
        //            if (choise == 1)
        //            {
        //                //Lấy ký tự đặc biệt
        //                rndString = rndString + s_SpecialChar.Substring(r.Next(0, s_SpecialChar.Length - 1), 1);
        //                isSpecial = true;
        //            }
        //            else if (choise == 2)
        //            {
        //                //Lấy ký tự số
        //                rndString = rndString + s_Digit.Substring(r.Next(0, s_Digit.Length - 1), 1);
        //                isDigit = true;
        //            }
        //            else
        //            {
        //                rndString = rndString + s_Nomal.Substring(r.Next(0, s_Nomal.Length - 1), 1);
        //                //Lấy ký tự thường
        //            }
        //        }
        //        if (isSpecial && isDigit)
        //        {
        //            break;
        //        }
        //    }
        //    return rndString;
        //}
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

        public static bool ValidatePhoneNumber(string phoneNumber)
        {
            if (phoneNumber.Substring(0, 2).Equals("84") && phoneNumber.Length == 11)
            {
                return true;
            }
            else if (phoneNumber.Substring(0, 1).Equals("0") && phoneNumber.Length == 10)
            {
                return true;
            }
             else if (Regex.Match(phoneNumber, @"^[0-9]+$").Success)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool ValidateFileName(string extension)
        {
            return extension.Equals(".png") || extension.Equals(".jpg") || extension.Equals(".jpeg");
        }
    }
}
