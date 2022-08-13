using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace SysFrameworks
{
    public static class Extensions
    {
        #region Extension for string
        public static string ToHex(this string value, Encoding enc)
        {
            // Encode the array of chars.
            StringBuilder returnString = new StringBuilder(string.Empty);
            foreach (var byteValue in enc.GetBytes(value))
                returnString.Append(String.Format("{0:X2}", byteValue));
            return returnString.ToString();
        }
        /// <summary>
        /// Trả lại lỗi thân thiện với người dùng
        /// </summary>
        public static string ToFriendlyErrorMsg(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return "";
            if (value.IndexOf("unique") > 0)
            {
                return "Dữ liệu đã tồn tại trong hệ thống!";
            }else  if (value.IndexOf("DELETE statement") > 0)
            {
                return "Dữ liệu đã được sử dụng. Không thể xóa được!";
            }
            return value;
        }
        /// <summary>
        /// Return null nếu xâu rỗng hoặc null
        /// </summary>
        public static string ToNullIfNullOrEmpty(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return null;
            return value;
        }
        /// <summary>
        /// Nâng hoa ký tự đầu tiên của một xâu
        /// </summary>
        public static string UppercaseFirstLetter(this string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                char[] array = value.ToCharArray();
                array[0] = char.ToUpper(array[0]);
                return new string(array);
            }
            return value;
        }
        /// <summary>
        /// Đếm số từ của một xâu
        /// </summary>
        public static int WordCount(this string value)
        {
            return value.Split(new char[] { ' ', '.', '?', ',', '!', ':' }, StringSplitOptions.RemoveEmptyEntries).Length;
        }

        public static bool ContainsUnicodeCharacter(this string str)
        {
            for (int i = 0, n = str.Length; i < n; i++)
            {
                if (str[i] > 127)
                {
                    return true;
                }
            }
            return false;
        }

        public static int GetLength(this string str)
        {
            char[] arr = new char[] { '|', '^', '€', '{', '}', '[', ']', '~', '\\', '\n' };
            int length = 0;
            for (var i = 0; i < str.Length; i++)
            {
                length += arr.Contains(str[i]) ? 2 : 1;
            }
            return length;
        }

        public static int GetMessageCount(this string str)
        {
            int letter = GetLength(str);
            int letterPermessage = 0;
            if (!str.ContainsUnicodeCharacter())
            {
                if (letter <= 160)
                    letterPermessage = 160;
                else
                    letterPermessage = 153;
            }
            else
            {
                if (letter <= 70)
                    letterPermessage = 70;
                else
                    letterPermessage = 67;
            }
            double message = Math.Floor(Convert.ToDouble(letter) / letterPermessage);
            var div = letter % letterPermessage;
            if (div > 0) message += 1;
            if (message == 0) message = 1;
            return Convert.ToInt32(message);
        }
        /// <summary>
        /// Kiểm tra một xâu có phải là số hay không. Trả về true nếu là số, không thì ngược lại
        /// </summary>
        public static bool IsNumeric(this string value)
        {
            double output;
            return double.TryParse(value, out output);
        }

        /// <summary>
        /// Kiểm tra một xâu có phải là bolean hay khoong;
        /// </summary>
        public static bool IsBolean(this string value)
        {
            bool output;
            return bool.TryParse(value, out output);
        }

        /// <summary>
        /// Convert 1 xâu thành 1 số nguyên
        /// </summary>
        public static int ToInt(this string value)
        {
            try
            {
                return int.Parse(value);
            }
            catch { return 0; }
        }

        /// <summary>
        /// Convert 1 xâu thành 1 số nguyên kiểu long
        /// </summary>
        public static long ToLong(this string value)
        {
            try
            {
                return long.Parse(value);
            }
            catch { return (long)0; }
        }

        /// <summary>
        /// Convert 1 xâu thành 1 số nguyên kiểu byte
        /// </summary>
        public static byte ToByte(this string value)
        {
            try
            {
                return byte.Parse(value);
            }
            catch { return (byte)0; }
        }

        /// <summary>
        /// Convert 1 xâu thành 1 số nguyên kiểu decimal
        /// </summary>
        public static decimal ToDecimal(this string value)
        {
            try
            {
                return decimal.Parse(value);
            }
            catch { return (decimal)0; }
        }

        /// <summary>
        /// Kiểm tra một xâu có phải là Email không
        /// </summary>
        public static bool IsEmail(this string value)
        {
            Regex reg = new Regex(@"^[a-z0-9_]+(?:\.[a-z0-9]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])$", RegexOptions.IgnoreCase);
            try
            {
                return reg.IsMatch(value);
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// Kiểm tra một xâu có phải là URL không
        /// </summary>
        public static bool IsURL(this string value)
        {
            Regex reg = new Regex(@"^(http|https|ftp)\://[a-zA-Z0-9\-\.]+\.[a-zA-Z]{2,3}(:[a-zA-Z0-9]*)?/?([a-zA-Z0-9\-\._\?\,\'/\\\+&%\$#\=~])*[^\.\,\)\(\s]$", RegexOptions.IgnoreCase);
            try
            {
                return reg.IsMatch(value);
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// Kiểm tra một xâu có phải là DateTime không
        /// </summary>
        public static bool IsDateTime(this string value)
        {
            DateTime dt;
            return DateTime.TryParse(value, out dt);
        }
        /// <summary>
        /// Kiểm tra một xâu có phải là số la mã không
        /// </summary>
        public static bool IsRomanNumber(this string value)
        {
            string[] soThuTuLaMa = {"I","II","III","IV","V","VI","VII","VIII","IX","X",
                                                 "XI","XII","XIII","XIV","XV","XVI","XVII","XVIII","XIX","XX",
                                                 "XXI","XXII","XXIII","XXIV","XXV","XXVI","XXVII","XXVIII","XXIX","XXX"};
            if (soThuTuLaMa.Contains(value)) return true;
            return false;
        }
        /// <summary>
        /// Chuẩn hóa một xâu
        /// </summary>
        public static string ToNormalString(this string value)
        {
            StringBuilder retValue = new StringBuilder(string.Empty);
            if (!string.IsNullOrEmpty(value))
            {
                string[] array = value.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string item in array)
                {
                    retValue.Append(item + " ");
                }
            }
            return retValue.ToString().TrimEnd();
        }
        /// <summary>
        /// Chuẩn hóa 1 số điện thoại (ví dụ: 841682821740)
        /// </summary>
        public static string ToNormalPhoneNumber(this string value)
        {
            string normalPhoneNumber = string.Empty;
            foreach (char item in value)
            {
                if (item >= '0' && item <= '9')
                    normalPhoneNumber += item;
            }
            //Kiểm tra 2 ký tự đầu, nếu không phải là 84 thì xử lý
            if (!normalPhoneNumber.StartsWith("84"))
            {
                //Kiểm tra 1 ký tự đầu, nếu là ký tự 0 thì cắt đi
                if (normalPhoneNumber.StartsWith("0"))
                {
                    normalPhoneNumber = normalPhoneNumber.Substring(1);
                }
                //Thêm ký tự 84 vào trước
                if (!string.IsNullOrWhiteSpace(normalPhoneNumber))
                {
                    normalPhoneNumber = "84" + normalPhoneNumber;
                }
            }
            else if (normalPhoneNumber.Length <= 9)
            {
                //Thêm ký tự 84 vào trước
                if (!string.IsNullOrWhiteSpace(normalPhoneNumber))
                {
                    normalPhoneNumber = "84" + normalPhoneNumber;
                }
            }
            return normalPhoneNumber;
        }
        /// <summary>
        /// Chuyển một xâu tiếng Việt có dấu thành không dấu (Nguồn: internet)
        /// </summary>
        public static string ToVietnameseWithoutAccent(this string text)
        {
            if (!string.IsNullOrEmpty(text))
            {
                System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(@"\p{IsCombiningDiacriticalMarks}+");
                string strFormD = text.Normalize(System.Text.NormalizationForm.FormD);
                return regex.Replace(strFormD, String.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D');
            }
            return text;
        }
        /// <summary>
        /// Mã hóa MD5
        /// </summary>
        public static string ToMD5Hash(this string value)
        {
            using (MD5 md5Hash = MD5.Create())
            {
                byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(value));
                StringBuilder sBuilder = new StringBuilder();
                for (int i = 0; i < data.Length; i++)
                {
                    sBuilder.Append(data[i].ToString("x2"));
                }
                return sBuilder.ToString();
            }
        }

        /// <summary>
        /// Mã hóa dữ liệu theo RSA
        /// </summary>
        /// <param name="_dataEncrypt">Dữ liệu cần mã hóa</param>
        /// <returns>Dữ liệu đã mã hóa</returns>
        public static string ToRsaEncrypt(this string value, string publicKeyPath)
        {
            RSACryptoServiceProvider rsa = CreateRSACryptoServiceProvider();
            using (System.IO.StreamReader reader = new System.IO.StreamReader(publicKeyPath))
            {
                string publicOnlyKeyXML = reader.ReadToEnd();
                rsa.FromXmlString(publicOnlyKeyXML);
                reader.Close();
            }
            //read plaintext, encrypt it to ciphertext

            byte[] plainbytes = System.Text.Encoding.UTF8.GetBytes(value);
            byte[] cipherbytes = rsa.Encrypt(plainbytes, false);
            return Convert.ToBase64String(cipherbytes);
        }

        /// <summary>
        /// Giải mã dữ liệu theo RSA
        /// </summary>
        /// <param name="_dataDecrypt"></param>
        /// <returns></returns>
        public static string ToRsaDecrypt(this string value, string privateKeyPath)
        {
            RSACryptoServiceProvider rsa = CreateRSACryptoServiceProvider();
            byte[] cipherbytes = Convert.FromBase64String(value);
            using (System.IO.StreamReader reader = new System.IO.StreamReader(privateKeyPath))
            {
                string publicPrivateKeyXML = reader.ReadToEnd();
                rsa.FromXmlString(publicPrivateKeyXML);
                reader.Close();
            }
            //read ciphertext, decrypt it to plaintext
            byte[] plain = rsa.Decrypt(cipherbytes, false);
            return System.Text.Encoding.UTF8.GetString(plain);

        }

        /// <summary>
        /// Tạo đối tượng RSACryptoServiceProvider
        /// </summary>
        /// <returns></returns>
        private static RSACryptoServiceProvider CreateRSACryptoServiceProvider()
        {
            CspParameters cspParams = new CspParameters(1, "VTCOnlinePassContainerKey");
            cspParams.Flags = CspProviderFlags.UseUserProtectedKey;
            cspParams.ProviderName = "Microsoft Strong Cryptographic Provider";
            return new RSACryptoServiceProvider(cspParams);
        }
        #endregion

        #region Extension for numeric
        public static decimal Round(this decimal value)
        {
            return Math.Round(value, MidpointRounding.AwayFromZero);
        }

        public static int? ToNullableInt(this string value)
        {
            int outResult;
            if (int.TryParse(value, out outResult))
                return outResult;
            return null;
        }

        public static long? ToNullableLong(this string value)
        {
            long outResult;
            if (long.TryParse(value, out outResult))
                return outResult;
            return null;
        }

        public static byte? ToNullableByte(this string value)
        {
            byte outResult;
            if (byte.TryParse(value, out outResult))
                return outResult;
            return null;
        }

        public static decimal? ToNullableDecimal(this string value)
        {
            decimal outResult;
            if (decimal.TryParse(value, out outResult))
                return outResult;
            return null;
        }

        public static bool? ToNullableBoolean(this string value)
        {
            bool outResult;
            if (bool.TryParse(value, out outResult))
                return outResult;
            return null;
        }

        public static Guid? ToNullableGuid(this string value)
        {
            Guid outResult;
            if (Guid.TryParse(value, out outResult))
                return outResult;
            return null;
        }

        public static string ReadMoney(this decimal gNum)
        {
            if (gNum == 0)
                return "Không đồng";

            string lso_chu = "";
            string tach_mod = "";
            string tach_conlai = "";
            decimal Num = Math.Round(gNum, 0);
            string gN = Convert.ToString(Num);
            int m = Convert.ToInt32(gN.Length / 3);
            int mod = gN.Length - m * 3;
            string dau = "[+]";

            // Dau [+ , - ]
            if (gNum < 0)
                dau = "[-]";
            dau = "";

            // Tach hang lon nhat
            if (mod.Equals(1))
                tach_mod = "00" + Convert.ToString(Num.ToString().Trim().Substring(0, 1)).Trim();
            if (mod.Equals(2))
                tach_mod = "0" + Convert.ToString(Num.ToString().Trim().Substring(0, 2)).Trim();
            if (mod.Equals(0))
                tach_mod = "000";
            // Tach hang con lai sau mod :
            if (Num.ToString().Length > 2)
                tach_conlai = Convert.ToString(Num.ToString().Trim().Substring(mod, Num.ToString().Length - mod)).Trim();

            ///don vi hang mod
            int im = m + 1;
            if (mod > 0)
                lso_chu = Tach(tach_mod).ToString().Trim() + " " + Donvi(im.ToString().Trim());
            /// Tach 3 trong tach_conlai

            int i = m;
            int _m = m;
            int j = 1;
            string tach3 = "";
            string tach3_ = "";

            while (i > 0)
            {
                tach3 = tach_conlai.Trim().Substring(0, 3).Trim();
                tach3_ = tach3;
                lso_chu = lso_chu.Trim() + " " + Tach(tach3.Trim()).Trim();
                m = _m + 1 - j;
                if (!tach3_.Equals("000"))
                    lso_chu = lso_chu.Trim() + " " + Donvi(m.ToString().Trim()).Trim();
                tach_conlai = tach_conlai.Trim().Substring(3, tach_conlai.Trim().Length - 3);

                i = i - 1;
                j = j + 1;
            }
            if (lso_chu.Trim().Substring(0, 1).Equals("k"))
                lso_chu = lso_chu.Trim().Substring(10, lso_chu.Trim().Length - 10).Trim();
            if (lso_chu.Trim().Substring(0, 1).Equals("l"))
                lso_chu = lso_chu.Trim().Substring(2, lso_chu.Trim().Length - 2).Trim();
            if (lso_chu.Trim().Length > 0)
                lso_chu = dau.Trim() + " " + lso_chu.Trim().Substring(0, 1).Trim().ToUpper() + lso_chu.Trim().Substring(1, lso_chu.Trim().Length - 1).Trim() + " đồng chẵn.";

            return lso_chu.ToString().Trim();
        }

        private static string Chu(string gNumber)
        {
            string result = "";
            switch (gNumber)
            {
                case "0":
                    result = "không";
                    break;
                case "1":
                    result = "một";
                    break;
                case "2":
                    result = "hai";
                    break;
                case "3":
                    result = "ba";
                    break;
                case "4":
                    result = "bốn";
                    break;
                case "5":
                    result = "năm";
                    break;
                case "6":
                    result = "sáu";
                    break;
                case "7":
                    result = "bảy";
                    break;
                case "8":
                    result = "tám";
                    break;
                case "9":
                    result = "chín";
                    break;
            }
            return result;
        }

        private static string Donvi(string so)
        {
            string Kdonvi = "";

            if (so.Equals("1"))
                Kdonvi = "";
            if (so.Equals("2"))
                Kdonvi = "nghìn";
            if (so.Equals("3"))
                Kdonvi = "triệu";
            if (so.Equals("4"))
                Kdonvi = "tỷ";
            if (so.Equals("5"))
                Kdonvi = "nghìn tỷ";
            if (so.Equals("6"))
                Kdonvi = "triệu tỷ";
            if (so.Equals("7"))
                Kdonvi = "tỷ tỷ";

            return Kdonvi;
        }

        private static string Tach(string tach3)
        {
            string Ktach = "";
            if (tach3.Equals("000"))
                return "";
            if (tach3.Length == 3)
            {
                string tr = tach3.Trim().Substring(0, 1).ToString().Trim();
                string ch = tach3.Trim().Substring(1, 1).ToString().Trim();
                string dv = tach3.Trim().Substring(2, 1).ToString().Trim();
                if (tr.Equals("0") && ch.Equals("0"))
                    Ktach = " không trăm lẻ " + Chu(dv.ToString().Trim()) + " ";
                if (!tr.Equals("0") && ch.Equals("0") && dv.Equals("0"))
                    Ktach = Chu(tr.ToString().Trim()).Trim() + " trăm ";
                if (!tr.Equals("0") && ch.Equals("0") && !dv.Equals("0"))
                    Ktach = Chu(tr.ToString().Trim()).Trim() + " trăm lẻ " + Chu(dv.Trim()).Trim() + " ";
                if (tr.Equals("0") && Convert.ToInt32(ch) > 1 && Convert.ToInt32(dv) > 0 && !dv.Equals("5"))
                    Ktach = " không trăm " + Chu(ch.Trim()).Trim() + " mươi " + Chu(dv.Trim()).Trim() + " ";
                if (tr.Equals("0") && Convert.ToInt32(ch) > 1 && dv.Equals("0"))
                    Ktach = " không trăm " + Chu(ch.Trim()).Trim() + " mươi ";
                if (tr.Equals("0") && Convert.ToInt32(ch) > 1 && dv.Equals("5"))
                    Ktach = " không trăm " + Chu(ch.Trim()).Trim() + " mươi lăm ";
                if (tr.Equals("0") && ch.Equals("1") && Convert.ToInt32(dv) > 0 && !dv.Equals("5"))
                    Ktach = " không trăm mười " + Chu(dv.Trim()).Trim() + " ";
                if (tr.Equals("0") && ch.Equals("1") && dv.Equals("0"))
                    Ktach = " không trăm mười ";
                if (tr.Equals("0") && ch.Equals("1") && dv.Equals("5"))
                    Ktach = " không trăm mười lăm ";
                if (Convert.ToInt32(tr) > 0 && Convert.ToInt32(ch) > 1 && Convert.ToInt32(dv) > 0 && !dv.Equals("5"))
                    Ktach = Chu(tr.Trim()).Trim() + " trăm " + Chu(ch.Trim()).Trim() + " mươi " + Chu(dv.Trim()).Trim() + " ";
                if (Convert.ToInt32(tr) > 0 && Convert.ToInt32(ch) > 1 && dv.Equals("0"))
                    Ktach = Chu(tr.Trim()).Trim() + " trăm " + Chu(ch.Trim()).Trim() + " mươi ";
                if (Convert.ToInt32(tr) > 0 && Convert.ToInt32(ch) > 1 && dv.Equals("5"))
                    Ktach = Chu(tr.Trim()).Trim() + " trăm " + Chu(ch.Trim()).Trim() + " mươi lăm ";
                if (Convert.ToInt32(tr) > 0 && ch.Equals("1") && Convert.ToInt32(dv) > 0 && !dv.Equals("5"))
                    Ktach = Chu(tr.Trim()).Trim() + " trăm mười " + Chu(dv.Trim()).Trim() + " ";

                if (Convert.ToInt32(tr) > 0 && ch.Equals("1") && dv.Equals("0"))
                    Ktach = Chu(tr.Trim()).Trim() + " trăm mười ";
                if (Convert.ToInt32(tr) > 0 && ch.Equals("1") && dv.Equals("5"))
                    Ktach = Chu(tr.Trim()).Trim() + " trăm mười lăm ";

            }


            return Ktach;

        }
        #endregion

        #region Extension for DateTime

        /// <summary>
        /// Cộng ngày tháng với quy ước 1 tháng có 30 ngày
        /// </summary>
        /// <param name="values">Số ngày cần cộng thêm</param>
        /// <returns></returns>
        public static DateTime AddDaysRound30(this DateTime d, int values)
        {
            int months = (values - 1) / 30;
            int days = (values - 1) % 30;
            DateTime retValue = d.AddMonths(months);
            if (retValue.Day + days <= new DateTime(retValue.Year, retValue.Month, 1).AddMonths(1).AddDays(-1).Day)
            {
                retValue = retValue.AddDays(days);
            }
            else
            {
                days = retValue.Day + days - 30;
                retValue = new DateTime(retValue.Year, retValue.Month + 1, days);
            }
            return retValue;
        }

        /// <summary>
        /// Trả về số ngày của tháng với quy ước một tháng có 30 ngày.
        /// </summary>
        public static int GetDayRound30(this DateTime d)
        {
            DateTime tmp = new DateTime(d.Year, d.Month, 1, 0, 0, 0);
            tmp = tmp.AddMonths(1).AddDays(-1);
            if (tmp.Day == d.Day) return 30;
            return d.Day;
        }

        /// <summary>
        /// Chuyển 1 xâu về kiểu dữ liệu DateTime
        /// </summary>
        public static DateTime? ToNullableDateTime(this string value)
        {
            DateTime outResult;
            if (DateTime.TryParse(value, out outResult))
                return outResult;
            return null;
        }

        public static DateTime? ToNullableDate(this string value)
        {
            DateTime outResult;
            if (DateTime.TryParse(value, out outResult))
                return outResult.Date;
            return null;
        }
        public static string ToNotNullableString(this string value)
        {
            if (value == null) return "";
            return value;
        }
        public static DateTime ToFirstDayOfWeek(this DateTime date, DayOfWeek firstDayOfWeek)
        {
            return date.AddDays(firstDayOfWeek - date.DayOfWeek);
        }

        public static DateTime ToLastDayOfWeek(this DateTime date, DayOfWeek firstDayOfWeek)
        {
            return ToFirstDayOfWeek(date, firstDayOfWeek).AddDays(6);
        }

        public static DateTime ToFirstDayOfMonth(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, 1, 0, 0, 0);
        }

        public static DateTime ToLastDayOfMonth(this DateTime date)
        {
            int day = DateTime.DaysInMonth(date.Year, date.Month);
            return new DateTime(date.Year, date.Month, day, date.Hour, date.Minute, date.Second, date.Millisecond);
        }

        public static DateTime ToFirstDayOfQuarter(this DateTime date)
        {
            int month = (((date.Month - 1) / 3 + 1) - 1) * 3 + 1;
            return new DateTime(date.Year, month, 1);
        }

        public static DateTime ToLastDayOfQuarter(this DateTime date)
        {
            DateTime firstDayOfQuarter = date.ToFirstDayOfQuarter();
            return firstDayOfQuarter.AddMonths(3).AddDays(-1);
        }

        public static DateTime ToFirstDayOfYear(this DateTime date)
        {
            return new DateTime(date.Year, 1, 1, date.Hour, date.Minute, date.Second, date.Millisecond);
        }

        public static DateTime ToLastDayOfYear(this DateTime date)
        {
            return new DateTime(date.Year, 12, 31, date.Hour, date.Minute, date.Second, date.Millisecond);
        }

        public static DateTime ToFirstDayOfFinancialYear(this DateTime date, int firstMonthOfFinancialYear)
        {
            int year = date.Year;
            if (date.Month < firstMonthOfFinancialYear) year--;
            return new DateTime(year, firstMonthOfFinancialYear, 1, date.Hour, date.Minute, date.Second, date.Millisecond);
        }

        public static DateTime ToLastDayOfFinancialYear(this DateTime date, int firstMonthOfFinancialYear)
        {
            return date.ToFirstDayOfFinancialYear(firstMonthOfFinancialYear).AddYears(1).AddDays(-1);
        }

        public static int ToWeekOfYear(this DateTime date, DayOfWeek firstDayOfWeek)
        {
            DateTimeFormatInfo dfi = DateTimeFormatInfo.CurrentInfo;
            Calendar cal = dfi.Calendar;
            return cal.GetWeekOfYear(date, CalendarWeekRule.FirstFourDayWeek, firstDayOfWeek);
        }

        public static int ToWeekOfMonth(this DateTime date, DayOfWeek firstDayOfWeek)
        {
            DateTime first = date.ToFirstDayOfWeek(firstDayOfWeek).AddDays(3).ToFirstDayOfMonth();
            return date.ToWeekOfYear(firstDayOfWeek) - first.ToWeekOfYear(firstDayOfWeek) + 1;
        }

        public static int ToQuarter(this DateTime date, int firstMonthOfFinancialYear)
        {
            int m = date.Month;
            if (m < firstMonthOfFinancialYear)
            {
                m += 12;
            }
            return (m - firstMonthOfFinancialYear) / 3 + 1;
        }
        #endregion
    }
}
