using BugTracker.Helpers;
using BugTracker.Models;
using System;
using System.Configuration;
using System.Diagnostics;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web.Mvc;
using BugTracker.ViewModels;

namespace BugTracker.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            MainDashboardViewModel viewModel = new MainDashboardViewModel();
            return View(viewModel);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Ira's Bug Tracker.";

            return View();
        }

        public ActionResult Contact()
        {
            var emailModel = new EmailModel
            {
                FromName = "Hiring Manager",
                FromEmail = "hiring.manager@foo.com",
                Subject = "Here's your job offer",
                Body = "100K per year enough?"
            };
            return View(emailModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Contact(EmailModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var strFormat = "<p>You have a message From: <bold>{0}</bold> ({1})</p><p>Message:</p><p>{2}</p>";

                    var emailFrom = ConfigurationManager.AppSettings["emailFrom"];
                    var emailTo = ConfigurationManager.AppSettings["emailTo"];

                    MailMessage mailMessage = new MailMessage(emailFrom, emailTo);
                    mailMessage.Subject = "IraNye Site Contact-Me Message: " + model.Subject;
                    mailMessage.Body = string.Format(strFormat, model.FromName, model.FromEmail, model.Body);
                    mailMessage.IsBodyHtml = true;

                    var svc = new PersonalEmail();
                    await svc.SendAsync(mailMessage);

                    return RedirectToAction("Index", "Home");
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    await Task.FromResult(0);
                }
            }

            return View(model);
        }
    }
}