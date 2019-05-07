using System;
using System.Net.Mail;
using System.Threading.Tasks;

namespace LoveDotNet.Helpers
{
    public static class EmailHelper
    {
        private static string mailFrom = "jypjypjypjyp@hotmail.com";
        private static string mailFromAccount = "jypjypjypjyp@hotmail.com";
        private static string mailPassword = "jyp258321654";

        private static SmtpClient _clinet;
        private static SmtpClient client
        {
            get
            {
                if (_clinet == null)
                {
                    _clinet = new SmtpClient
                    {
                        Port = 587,
                        UseDefaultCredentials = false,
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        Host = "smtp.live.com",
                        EnableSsl = true,
                        Credentials = new System.Net.NetworkCredential(mailFromAccount, mailPassword)
                    };
                }
                return _clinet;
            }
        }
        public static async Task<bool> SendAsync(string to, string subject, string html)
        {
            var mymail = new MailMessage();
            mymail.From = new System.Net.Mail.MailAddress(mailFrom);
            mymail.To.Add(to);
            mymail.Subject = subject;
            mymail.Body = html;
            mymail.IsBodyHtml = true;
            try
            {
                await client.SendMailAsync(mymail);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
