using System;
using System.Collections.Generic;
using System.Text;

namespace SysFrameworks
{
    public class Url
    {
        public static string to_Get_Param(DataCollections cols)
        {
            String val = "";
            for (int i = 0; i < cols.Count(); ++i)
            {
                if (cols[i].DataValue != null && cols[i].DataValue.ToString() != "")
                {
                    if (cols[i].DataType == DataTypes.DateTime)
                    {
                        val = val + cols[i].DataField.ToLower() + "=" + Convert.ToDateTime(cols[i].DataValue).ToString("yyyy/MM/dd HH:mm:ss");
                    }
                    else
                    {
                        val = val + cols[i].DataField.ToLower() + "=" + cols[i].DataValue;
                    }
                    if (i < cols.Count() - 1)
                    {
                        val = val + "&";
                    }
                }
                else if (cols[i].DataValue == null && cols[i].DataType == DataTypes.DateTime)
                {
                    //Datetime đưa giá trị mặc định vào để không lấy giá trị mặc định
                    val = val + cols[i].DataField.ToLower() + "=" + cols[i].DataValue;
                    if (i < cols.Count() - 1)
                    {
                        val = val + "&";
                    }
                }

            }
            return val;
        }
    }
}
