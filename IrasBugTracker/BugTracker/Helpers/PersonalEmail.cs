using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;
using Microsoft.AspNet.Identity;

namespace BugTracker.Helpers
{
    public class PersonalEmail : IIdentityMessageService
    {
        public async Task SendAsync(MailMessage message)
        {
            var user = WebConfigurationManager.AppSettings["emailFrom"];
            var password = WebConfigurationManager.AppSettings["password"];
            var host = WebConfigurationManager.AppSettings["host"];
            var port = Convert.ToInt32(WebConfigurationManager.AppSettings["port"]);

            using (var smtp = new SmtpClient()
            {
                Host = host,
                Port = port,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(user, password)
            })
            {
                try
                {
                    await smtp.SendMailAsync(message);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    await Task.FromResult(0);
                }
            }
        }

        public void Send(MailMessage message)
        {
            var user = WebConfigurationManager.AppSettings["emailFrom"];
            var password = WebConfigurationManager.AppSettings["password"];
            var host = WebConfigurationManager.AppSettings["host"];
            var port = Convert.ToInt32(WebConfigurationManager.AppSettings["port"]);

            using (var smtp = new SmtpClient()
            {
                Host = host,
                Port = port,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(user, password)
            })
            {
                try
                {
                    smtp.Send(message);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            }
        }

        public async Task SendAsync(IdentityMessage message)
        {
            var senderEmail = WebConfigurationManager.AppSettings["emailFrom"];
            var password = WebConfigurationManager.AppSettings["password"];
            var host = WebConfigurationManager.AppSettings["host"];
            var port = Convert.ToInt32(WebConfigurationManager.AppSettings["port"]);

            using (var smtp = new SmtpClient()
            {
                Host = host,
                Port = port,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(senderEmail, password)
            })
            {
                try
                {
                    var sender = $"{senderEmail}<{senderEmail}>";
                    MailMessage mailMessage = new MailMessage(sender, message.Destination);
                    mailMessage.Subject = message.Subject;
                    mailMessage.Body = message.Body;
                    mailMessage.IsBodyHtml = true;
                    await smtp.SendMailAsync(mailMessage);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    await Task.FromResult(0);
                }
            }
        }
    }
}
