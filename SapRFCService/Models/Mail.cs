using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace SapRFCService.Models
{
    public class Mail
    {

        public static void Send(string strMailTitle, string strMailContent)
        {
            string MailServer = System.Web.Configuration.WebConfigurationManager.AppSettings["MailServer"].ToString();
            string Reciever = System.Web.Configuration.WebConfigurationManager.AppSettings["Reciever"].ToString();

            using (SmtpClient client = new SmtpClient(MailServer))
            {
                string to = Reciever;
                string from = string.Format("系統通知信- 請勿回覆<{0}>", "service@eslite.com");
                MailMessage message = new MailMessage(from, to);
                message.Subject = strMailTitle;
                message.Body = strMailContent;
                client.UseDefaultCredentials = true;
                client.Send(message);
            }
        }
    }
}