using SysFrameworks;
using System;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace JoinReward.Models
{
    public class MailService
    {
        public static void sendMail(string mailTo, string password, string userName)
        {
            using (MailMessage newMail = new MailMessage())
            {
                
                    newMail.From = new MailAddress("support@vuikhoetiennghi.vn");
                    newMail.To.Add(mailTo);
                    newMail.Subject = "Thông tin tài khoản";
                    newMail.BodyEncoding = Encoding.UTF8;
                    newMail.SubjectEncoding = Encoding.UTF8;
                    newMail.Body = "<html><body><font face='Courier New, Courier, mono' size='-1'>";
                    newMail.Body += String.Format(" <br />Xin chào,<br /><br />Tài khoản của bạn là: < {0} >", userName);
                    newMail.Body += "<br /><br />";
                    newMail.Body += String.Format(" Mật khẩu của bạn là : < {0} ><br /><br />", password);

                    newMail.IsBodyHtml = true;

                    var client = new SmtpClient("pro17.emailserver.vn", 465)
                    {
                        Credentials = new NetworkCredential("support", "q7kJ5Eld(EvL"),
                        EnableSsl = true
                    };

                    client.Send(newMail);
                }
            
        }
    }
}
