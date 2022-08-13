// VBConversions Note: VB project level imports

using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

// End of VB project level imports


// <summary>
// Sử dụng để mã hóa, giải mã chuỗi ký tự
// Gán quyền cho tệp tin
// </summary>
// <remarks></remarks>

namespace SysFrameworks
{
    public class Crypto
    {
        private static string _vstrEncryptionKey = "Panasonic";
        /// <summary>
        /// EncryptString128Bit
        /// </summary>
        /// <param name="vstrTextToBeEncrypted"></param>
        /// <param name="vstrEncryptionKey"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string EncryptString128Bit(string vstrTextToBeEncrypted)
        {
            string vstrEncryptionKey = _vstrEncryptionKey;
            byte[] bytValue = null;
            byte[] bytKey = null;
            var bytEncoded = new byte[] { };
            var bytIV = new[]
                            {
                                (byte) 121, (byte) 241, (byte) 10, (byte) 1, (byte) 132, (byte) 74, (byte) 11, (byte) 39
                                ,
                                (byte) 255, (byte) 91, (byte) 45, (byte) 78, (byte) 14, (byte) 211, (byte) 22, (byte) 62
                            };
            int intLength = default(int);
            int intRemaining = default(int);
            var objMemoryStream = new MemoryStream();
            CryptoStream objCryptoStream = default(CryptoStream);
            RijndaelManaged objRijndaelManaged = default(RijndaelManaged);


            //   **********************************************************************
            //   ******  Strip any null character from string to be encrypted    ******
            //   **********************************************************************

            vstrTextToBeEncrypted = StripNullCharacters(vstrTextToBeEncrypted);

            //   **********************************************************************
            //   ******  Value must be within ASCII range (i.e., no DBCS chars)  ******
            //   **********************************************************************

            bytValue = Encoding.ASCII.GetBytes(vstrTextToBeEncrypted.ToCharArray());

            intLength = vstrEncryptionKey.Length;

            //   ********************************************************************
            //   ******   Encryption Key must be 256 bits long (32 bytes)      ******
            //   ******   If it is longer than 32 bytes it will be truncated.  ******
            //   ******   If it is shorter than 32 bytes it will be padded     ******
            //   ******   with upper-case Xs.                                  ******
            //   ********************************************************************

            if (intLength >= 32)
            {
                vstrEncryptionKey = vstrEncryptionKey.Substring(0, 32);
            }
            else
            {
                intLength = vstrEncryptionKey.Length;
                intRemaining = 32 - intLength;
                vstrEncryptionKey = vstrEncryptionKey + new string('X', intRemaining);
            }

            bytKey = Encoding.ASCII.GetBytes(vstrEncryptionKey.ToCharArray());

            objRijndaelManaged = new RijndaelManaged();

            //   ***********************************************************************
            //   ******  Create the encryptor and write value to it after it is   ******
            //   ******  converted into a byte array                              ******
            //   ***********************************************************************

            try
            {
                objCryptoStream = new CryptoStream(objMemoryStream, objRijndaelManaged.CreateEncryptor(bytKey, bytIV),
                                                   CryptoStreamMode.Write);
                objCryptoStream.Write(bytValue, 0, bytValue.Length);

                objCryptoStream.FlushFinalBlock();

                bytEncoded = objMemoryStream.ToArray();
                objMemoryStream.Close();
                objCryptoStream.Close();
            }
            catch
            {
            }

            //   ***********************************************************************
            //   ******   Return encryptes value (converted from  byte Array to   ******
            //   ******   a base64 string).  Base64 is MIME encoding)             ******
            //   ***********************************************************************

            return Convert.ToBase64String(bytEncoded);
        }

        /// <summary>
        /// DecryptString128Bit
        /// </summary>
        /// <param name="vstrStringToBeDecrypted"></param>
        /// <param name="vstrDecryptionKey"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string DecryptString128Bit(string vstrStringToBeDecrypted)
        {
            string vstrDecryptionKey = _vstrEncryptionKey;
            byte[] bytDataToBeDecrypted = null;
            byte[] bytTemp = null;
            var bytIV = new[]
                            {
                                (byte) 121, (byte) 241, (byte) 10, (byte) 1, (byte) 132, (byte) 74, (byte) 11, (byte) 39
                                ,
                                (byte) 255, (byte) 91, (byte) 45, (byte) 78, (byte) 14, (byte) 211, (byte) 22, (byte) 62
                            };
            var objRijndaelManaged = new RijndaelManaged();
            MemoryStream objMemoryStream = default(MemoryStream);
            CryptoStream objCryptoStream = default(CryptoStream);
            byte[] bytDecryptionKey = null;

            int intLength = default(int);
            int intRemaining = default(int);
            string strReturnString = string.Empty;

            //   *****************************************************************
            //   ******   Convert base64 encrypted value to byte array      ******
            //   *****************************************************************

            bytDataToBeDecrypted = Convert.FromBase64String(vstrStringToBeDecrypted);

            //   ********************************************************************
            //   ******   Encryption Key must be 256 bits long (32 bytes)      ******
            //   ******   If it is longer than 32 bytes it will be truncated.  ******
            //   ******   If it is shorter than 32 bytes it will be padded     ******
            //   ******   with upper-case Xs.                                  ******
            //   ********************************************************************

            intLength = vstrDecryptionKey.Length;

            if (intLength >= 32)
            {
                vstrDecryptionKey = vstrDecryptionKey.Substring(0, 32);
            }
            else
            {
                intLength = vstrDecryptionKey.Length;
                intRemaining = 32 - intLength;
                vstrDecryptionKey = vstrDecryptionKey + new string('X', intRemaining);
            }

            bytDecryptionKey = Encoding.ASCII.GetBytes(vstrDecryptionKey.ToCharArray());

            bytTemp = new byte[bytDataToBeDecrypted.Length + 1];

            objMemoryStream = new MemoryStream(bytDataToBeDecrypted);

            //   ***********************************************************************
            //   ******  Create the decryptor and write value to it after it is   ******
            //   ******  converted into a byte array                              ******
            //   ***********************************************************************

            try
            {
                objCryptoStream = new CryptoStream(objMemoryStream,
                                                   objRijndaelManaged.CreateDecryptor(bytDecryptionKey, bytIV),
                                                   CryptoStreamMode.Read);

                objCryptoStream.Read(bytTemp, 0, bytTemp.Length);

                objCryptoStream.FlushFinalBlock();
                objMemoryStream.Close();
                objCryptoStream.Close();
            }
            catch
            {
            }

            //   *****************************************
            //   ******   Return decypted value     ******
            //   *****************************************

            return StripNullCharacters(Encoding.ASCII.GetString(bytTemp));
        }

        /// <summary>
        /// StripNullCharacters
        /// </summary>
        /// <param name="vstrStringWithNulls"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string StripNullCharacters(string vstrStringWithNulls)
        {
            int intPosition = default(int);
            string strStringWithOutNulls = default(string);

            intPosition = 1;
            strStringWithOutNulls = vstrStringWithNulls;

            while (intPosition > 0)
            {
                intPosition = vstrStringWithNulls.IndexOf('\0', intPosition - 1) + 1;

                if (intPosition > 0)
                {
                    strStringWithOutNulls = (strStringWithOutNulls.Substring(0, intPosition - 1) + strStringWithOutNulls.Substring(intPosition, strStringWithOutNulls.Length - intPosition));
                }

                if (intPosition > strStringWithOutNulls.Length)
                {
                    break;
                }
            }

            return strStringWithOutNulls;
        }

        /// <summary>
        /// Add File Security
        /// </summary>
        /// <param name="FileName"></param>
        /// <param name="Account"></param>
        /// <param name="Rights"></param>
        /// <param name="ControlType"></param>
        /// <returns></returns>
        /// <remarks></remarks>        
    }
}