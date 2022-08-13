using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Xml;

namespace SysFrameworks
{

    public class SMSResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
    }
    public class SMSParameters
    {
        //CooperateID
        public int CooperateID { get; set; }
        //PublicKey
        public string StrPublicKey { get; set; }
        //PublicPrivateKey
        public string StrPublicPrivateKey { get; set; }

        public string ServiceNumber { get; set; }

        public string StransID { get; set; }
        public string CheckSum { get; set; }
        public string EncryptedMessage { get; set; }
        public SMSParameters(
            int cooperateID = 46043,
            string strPublicKey = "<BitStrength>512</BitStrength><RSAKeyValue><Modulus>0b5HE26o6KbQ2aWdxRP30QKMsV8iGHnJk9O73uHhDfMlccYBpFCaZ7XDDm3fMfSMUTcnLOuj1OkteVC1n0YQwQ==</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>",
            string strPublicPrivateKey = "<BitStrength>512</BitStrength><RSAKeyValue><Modulus>0b5HE26o6KbQ2aWdxRP30QKMsV8iGHnJk9O73uHhDfMlccYBpFCaZ7XDDm3fMfSMUTcnLOuj1OkteVC1n0YQwQ==</Modulus><Exponent>AQAB</Exponent><P>9WsRRo+m2IYFiGZFUIAv0EKAfl1hB8BWeqg8FlYfen0=</P><Q>2slt1xWLeENt7CGx8R54t8qRzaavQsV7A0FJmK0nvpU=</Q><DP>YswSbVSBN2NksM9zEgA4v182OSjAWg19Au3dGqTbEUU=</DP><DQ>W40uiXJailitBsKS03MM0NvTZ1r4u9gnQZVwVpMeroE=</DQ><InverseQ>oQxaqdn2WO048OnPSEs9HrpTkfCJLsSGO56d8Q+HrIE=</InverseQ><D>A3tlMq1joHkkfniBZgQu2QlxzIAH0OA0uH+LXufhFJotEBddMZ8PGdF//Dt74jYg3ADAoq0Bjo6WG40DTxOsAQ==</D></RSAKeyValue>",
            string serviceNumber = "Panasonic"
        )
        {
            this.CooperateID = cooperateID;
            this.StrPublicKey = strPublicKey;
            this.StrPublicPrivateKey = strPublicPrivateKey;
            this.ServiceNumber = serviceNumber;
            //
            this.StransID = Guid.NewGuid().ToString("N");
        }

        public void InitParameters(string[] smsNumbers, string message)
        {
            //Tạo dữ cấu trúc dữ liệu cho DataSet
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            dt.TableName = "SMSOut";

            DataColumn myDataColumn;

            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "DestAddr";
            dt.Columns.Add(myDataColumn);

            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "ShortCode";
            dt.Columns.Add(myDataColumn);

            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "Message";
            dt.Columns.Add(myDataColumn);

            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "CooperateGUID";
            dt.Columns.Add(myDataColumn);

            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "DeliverSmID";
            dt.Columns.Add(myDataColumn);

            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "CDRIndicator";
            dt.Columns.Add(myDataColumn);

            dt.Rows.Clear();
            //Sinh dữ liệu cho DataSet
            string[] arrayNumber = smsNumbers;
            for (int j = 0; j < arrayNumber.Length; j++)
            {
                DataRow dr;
                dr = dt.NewRow();
                dr["DestAddr"] = arrayNumber[j]; //Số khách hàng cần gửi tin nhắn
                dr["ShortCode"] = ServiceNumber; //Số dịch vụ dùng để gửi tin
                dr["Message"] = message; //Nội dung cần gửi                                                         
                dr["CooperateGUID"] = Guid.NewGuid().ToString("N"); //Tao GUID de tranh lap tin, su dung GUID nay de lay Report ve
                dr["DeliverSmID"] = "";
                dr["CDRIndicator"] = "FREE";

                dt.Rows.Add(dr);
            }

            ds.Tables.Add(dt);

            //Chuyển DataSet thành XML
            XmlElement xml = Xml.Serialize(ds);
            string bitStrengthString = StrPublicKey.Substring(0, StrPublicKey.IndexOf("</BitStrength>") + 14);
            string strPublicKeyWitoutBitStrength = StrPublicKey.Replace(bitStrengthString, "");
            int bitStrength = Convert.ToInt32(bitStrengthString.Replace("<BitStrength>", "").Replace("</BitStrength>", ""));
            string inputString = xml.OuterXml;
            //Sinh MD5 cho chuỗi xml - Hàm SendEncryptSMSMessage sẽ giải mã chuỗi XML sau đó kiểm tra CheckSum xem nội dung có bị thay đổi không
            CheckSum = Cryptography.getMd5Hash(inputString);
            //Mã hóa XML bằng PublicKey
            EncryptedMessage = Cryptography.EncryptString(inputString, bitStrength, strPublicKeyWitoutBitStrength);
        }
    }
}
