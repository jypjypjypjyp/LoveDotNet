//using System;
//using System.Net;
//using System.Net.Http;
//using System.Net.Mail;
//using LoveDotNet.Models;
//using Newtonsoft.Json;

//namespace Test
//{
//    class Program
//    {
//        static void Main(string[] args)
//        {
//            //Console.ReadKey();

//            var client = new SmtpClient("smtp.163.com", 465);
//            client.UseDefaultCredentials = false;
//            client.Credentials = new NetworkCredential("@qq.com", "zrxobgzqupqxggfc");
//            client.EnableSsl = true;

//            var mailMessage = new MailMessage();
//            mailMessage.From = new MailAddress("1104462345@qq.com");
//            mailMessage.To.Add("1017722789@qq.com");
//            mailMessage.Body = "Hello";
//            mailMessage.Subject = "subject";
//            client.Send(mailMessage);
//            //using (var db = new MyDBContext())
//            //{
//            //    for (int i = 4; i < 22; i++)
//            //    {
//            //        db.Articles.Add(new Article() { Title = "Hello", Time = DateTime.Now, Description = "HelloHelloHelloHelloHelloHelloHelloHelloHelloHelloHelloHelloHelloHelloHelloHelloHelloHelloHelloHelloHelloHelloHelloHelloHelloHello", UserId = 1 });
//            //        db.SaveChanges();
//            //        var a = db.Articles.Include(a=>a.Paragraphs).Where(a=>a.Id==i).FirstOrDefault();
//            //        Paragraph p1 = new Paragraph() { Number = 1, ArticleId = a.Id, Type = ParagraphType.Image, Content = "sample.jpg:<font color=\"red\">This is some text!</font>" };
//            //        a.Paragraphs.Add(p1);
//            //        Paragraph p2 = new Paragraph() { Number = 2, ArticleId = a.Id, Type = ParagraphType.Text, Content = "<font color=\"green\">This is some text!</font>" };
//            //        a.Paragraphs.Add(p2);
//            //        Paragraph p3 = new Paragraph() { Number = 3, ArticleId = a.Id, Type = ParagraphType.Image, Content = "sample2.jpg:<font color=\"blue\">This is some text!</font>" };
//            //        a.Paragraphs.Add(p3);
//            //        db.SaveChanges();
//            //    }
//            //}
//            //using (var Http = new HttpClient())
//            //{
//            //    Http.BaseAddress = new Uri("http://localhost:5000/");
//            //    var json = JsonConvert.SerializeObject(new User() { Id = 1, Email = "jjjj@ads.sd", Password = "123" });
//            //    var stringContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
//            //    var result = Http.PutAsync("api/Users/1", stringContent).Result;

//            //}
//        }
//    }
//}
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using System.IO;

namespace Test
{
    class Program
    {
        const string mailFrom = "jypjypjypjyp@hotmail.com";
        const string mailTo = "1104462345@qq.com";
        const string mailFromAccount = "jypjypjypjyp@hotmail.com";
        const string mailPassword = "jyp258321654";
        static void Main(string[] args)
        {
            TestSmtpClient();

            TestMailKit();

        }

        private static void TestMailKit()
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("jypjypjypjyp", mailFrom));
            message.To.Add(new MailboxAddress("1104462345", mailTo));
            message.Subject = string.Format("C#自动发送邮件测试 From geffzhang TO {0}", mailTo);

            var plain = new TextPart("plain")
            {
                Text = @"不好意思，我在测试程序，刚才把QQ号写错了，Sorry！"
            };
            var html = new TextPart("html")
            {
                Text = @"<p>Hey geffzhang<br>
<p>不好意思，我在测试程序，刚才把QQ号写错了，Sorry！<br>
<p>-- Geffzhang<br>"
            };
            // create an image attachment for the file located at path
            //var attachment = new MimePart("image", "png")
            //{
            //    ContentObject = new ContentObject(File.OpenRead(path), ContentEncoding.Default),
            //    ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
            //    ContentTransferEncoding = ContentEncoding.Base64,
            //    FileName = Path.GetFileName(path)
            //};

            var alternative = new Multipart("alternative");
            alternative.Add(plain);
            alternative.Add(html);

            // now create the multipart/mixed container to hold the message text and the
            // image attachment
            var multipart = new Multipart("mixed");
            multipart.Add(alternative);
            //multipart.Add(attachment);
            message.Body = multipart;

            using (var client = new MailKit.Net.Smtp.SmtpClient())
            {
                client.Connect("smtp.live.com", 587, false);

                // Note: since we don't have an OAuth2 token, disable
                // the XOAUTH2 authentication mechanism.
                client.AuthenticationMechanisms.Remove("XOAUTH2");

                // Note: only needed if the SMTP server requires authentication
                client.Authenticate(mailFromAccount, mailPassword);

                client.Send(message);
                client.Disconnect(true);
            }
        }

        private static void TestSmtpClient()
        {
            MailMessage mymail = new MailMessage();
            mymail.From = new System.Net.Mail.MailAddress(mailFrom);
            mymail.To.Add(mailTo);
            mymail.Subject = string.Format("C#自动发送邮件测试 From geffzhang TO {0}", mailTo);
            mymail.Body = @"<p>Hey geffzhang<br><p>不好意思，我在测试程序，刚才把QQ号写错了，Sorry！<br><p>-- Geffzhang<br>";
            mymail.IsBodyHtml = true;
            //mymail.Attachments.Add(new Attachment(path));

            System.Net.Mail.SmtpClient smtpclient = new System.Net.Mail.SmtpClient();
            smtpclient.Port = 587;
            smtpclient.UseDefaultCredentials = false;
            smtpclient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpclient.Host = "smtp.live.com";
            smtpclient.EnableSsl = true;
            smtpclient.Credentials = new System.Net.NetworkCredential(mailFromAccount, mailPassword);
            try
            {
                smtpclient.Send(mymail);
                Console.WriteLine("发送成功");


            }
            catch (Exception ex)
            {
                Console.WriteLine("发送邮件失败.请检查是否为qq邮箱，并且没有被防护软件拦截" + ex);

            }
        }
    }
}