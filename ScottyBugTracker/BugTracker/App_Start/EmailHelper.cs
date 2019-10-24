using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;
using BugTracker.Controllers;
using BugTracker.Models;
using Microsoft.Ajax.Utilities;

namespace BugTracker.App_Start
{
    public class EmailHelper
    {
        private static readonly string ConfiguredEmail = WebConfigurationManager.AppSettings["emailFrom"];

        public static async Task ComposeEmailAsync(EmailModel email)
        {
            try
            {
                var senderEmail = $"{email.FromEmail}<{ConfiguredEmail}>";
                var mailMsg = new MailMessage(senderEmail, ConfiguredEmail)
                {
                    Subject = email.Subject,
                    Body = $"<strong>{email.FromEmail} has sent you the following message</strong><hr/>{email.Body}",
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

        public static async Task ComposeEmailAsync(RegisterViewModel model, string callbackUrl)
        {
            try
            {
                var senderEmail = $"Web Admin<{ConfiguredEmail}>";
                var mailMsg = new MailMessage(senderEmail, ConfiguredEmail)
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
            try
            {
                var senderEmail = $"Web Admin<{ConfiguredEmail}>";
                var mailMsg = new MailMessage(senderEmail, ConfiguredEmail)
                {
                    Subject = "Reset Password",
                    Body = $"Please reset your password (at BugTracker) by clicking <a href=\"{callbackUrl}\"here</a>",
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
    }
}
