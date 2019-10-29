using System;
using System.Net.Mail;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;
using BugTracker.Models;

namespace BugTracker.Helpers
{
    public class EmailHelper
    {
        private static readonly string EmailSendFrom = WebConfigurationManager.AppSettings["emailFrom"];
        private static readonly string EmailSendTo = WebConfigurationManager.AppSettings["emailTo"];

        public static async Task ComposeEmailAsync(EmailModel email)
        {
            var strFormat = "<p>You have a message From: <bold>{0}</bold> ({1})</p><p>Message:</p><p>{2}</p>";

            try
            {
                var senderEmail = $"{email.FromEmail}<{EmailSendFrom}>";
                var mailMsg = new MailMessage(senderEmail, EmailSendTo);
                mailMsg.Subject = "IraNye Site Contact-Me Message from <strong>{email.FromEmail}</strong>: " + email.Subject;
                mailMsg.Body = string.Format(strFormat, email.FromName, email.FromEmail, email.Body);
                mailMsg.IsBodyHtml = true;

                var svc = new PersonalEmail();
                await svc.SendAsync(mailMsg);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                await Task.FromResult(0);
            }
        }

        public static async Task ComposeEmailAsync(RegisterViewModel model, string callbackUrl)
        {
            try
            {
                var senderEmail = $"Web Admin<{EmailSendFrom}>";
                var recipientEmail = model.Email;
                var mailMsg = new MailMessage(senderEmail, recipientEmail)
                {
                    Subject = "Confirm your Account",
                    Body = $"Please confirm your Account (at BugTracker) by clicking <a href=\"{callbackUrl}\"here</a>",
                    IsBodyHtml = true
                };

                var svc = new PersonalEmail();
                await svc.SendAsync(mailMsg);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                await Task.FromResult(0);
            }
        }

        public static async Task ComposeEmailAsync(ForgotPasswordViewModel model, string callbackUrl)
        {
            var strFormat = "<p>You have a message From: <bold>{0}</bold> ({1})</p><p>Message:</p><p>{2}</p>";
            var appName = "BugTracker";

            try
            {
                var senderEmail = $"{EmailSendFrom}<{EmailSendFrom}>";
                var recipientEmail = model.Email;
                var mailMsg = new MailMessage(senderEmail, recipientEmail);
                mailMsg.Subject = $"({appName}) Forgot Password";
                mailMsg.Body = callbackUrl;
                mailMsg.IsBodyHtml = true;

                var svc = new PersonalEmail();
                await svc.SendAsync(mailMsg);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                await Task.FromResult(0);
            }
        }
    }
}
