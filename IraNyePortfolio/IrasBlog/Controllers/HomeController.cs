using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Net.Mail;
using System.Security.Policy;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using IrasBlog.Models;
using PagedList;

namespace IrasBlog.Controllers
{
    [RequireHttps]
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: BlogPosts
        public ActionResult Index(int? page, string searchStr)
        
        {
            int pageSize = 3;
            int pageNumber = (page ?? 1);
            IQueryable<BlogPost> listOfPosts = IndexSearch(searchStr);
            return View(listOfPosts.Where(b => b.Published).OrderByDescending(b => b.Created).ToPagedList(pageNumber, pageSize));
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

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
            return View(new EmailModel());
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

                    var mailMessage = new MailMessage(emailFrom, emailTo);
                    mailMessage.Subject = "IraNye Site Contact-Me Message: " + model.Subject;
                    mailMessage.Body = string.Format(strFormat, model.FromName, model.FromEmail, model.Body);
                    mailMessage.IsBodyHtml = true;

                    var svc = new PersonalEmail();
                    await svc.SendAsync(mailMessage);

                    return RedirectToAction("Index", "Home");
                }
                catch(Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    await Task.FromResult(0);
                }
            }

            return View(model);
        }

        private IQueryable<BlogPost> IndexSearch(string searchStr)
        {
            if (String.IsNullOrWhiteSpace(searchStr))
            {
                return db.BlogPosts.AsQueryable();
            }

            return db.BlogPosts.AsQueryable()
                .Where(p => p.Title.Contains(searchStr)
                            || p.BlogPostBody.Contains(searchStr)
                            || p.Abstract.Contains(searchStr)
                            || p.Comments.Any(
                                c => c.CommentBody.Contains(searchStr) ||
                                     c.Author.FirstName.Contains(searchStr) ||
                                     c.Author.LastName.Contains(searchStr) ||
                                     c.Author.DisplayName.Contains(searchStr) ||
                                     c.Author.Email.Contains(searchStr)));
        }
    }
}