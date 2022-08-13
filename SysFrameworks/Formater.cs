using System;
using System.Collections.Generic;
using System.Text;

namespace SysFrameworks
{
    public class Formater
    {
        public static string FormatDate(object obj)
        {
            try
            {
                return Convert.ToDateTime(obj).ToString("dd/MM/yyyy");
            }
            catch
            {
                return "";
            }
        }
    }
}
