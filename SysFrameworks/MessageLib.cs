using System;
using System.Collections.Generic;
using System.Text;

namespace SysFrameworks
{
    public class MessageLib
    {
        public static string ToFriendlyMessage(string sMessage)
        {
            if (sMessage.IndexOf("insert the value NULL") > 0)
            {
                return "Hãy nhập dữ liệu bắt buộc!";
            }else if (sMessage.IndexOf("duplicate") > 0)
            {
                return "Dữ liệu đã tồn tại trong hệ thống!";
            }
            
            return sMessage;
        }
    }
}
