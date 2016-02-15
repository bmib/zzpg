using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;

namespace Solution.Common
{
    public class MailHelper
    {
        public static void SendEmail(string mailTitle, string mailBody, string mailTo)
        {
            SmtpClient mSmtpClient = new SmtpClient();
            mSmtpClient.Host = Constants.SendEmailSmtpServer;
            mSmtpClient.Port = Constants.SendEmailSmtpPort;
            mSmtpClient.UseDefaultCredentials = false;
            mSmtpClient.EnableSsl = false;
            mSmtpClient.Credentials = new System.Net.NetworkCredential(Constants.SendEmailUserName, Constants.SendEmailPassword);
            mSmtpClient.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;

            MailMessage mMailMessage = new MailMessage();
            mMailMessage.To.Add(mailTo);
            mMailMessage.From = new MailAddress(Constants.SendEmailAddress);
            mMailMessage.Subject = mailTitle;
            mMailMessage.Body = mailBody;
            mMailMessage.IsBodyHtml = true;
            mMailMessage.BodyEncoding = System.Text.Encoding.UTF8;
            mMailMessage.Priority = MailPriority.Normal;

            mSmtpClient.Send(mMailMessage);
        }
    }
}
