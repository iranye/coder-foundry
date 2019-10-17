using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IrasBlog.Models;
using PagedList;

namespace IrasBlog.Controllers
{
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
            ViewBag.Message = "Your contact page.";

            return View();
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