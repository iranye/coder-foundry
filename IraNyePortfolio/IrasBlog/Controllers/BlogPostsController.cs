﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using IrasBlog.Helpers;
using IrasBlog.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace IrasBlog.Controllers
{
    public class BlogPostsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: BlogPosts
        public ActionResult Index()
        {
            return View(db.BlogPosts.OrderByDescending(b => b.Created).ToList());
        }

        public ActionResult Details(string slug)
        {
            var blogPost = db.BlogPosts.FirstOrDefault(b => b.Slug == slug);
            if (blogPost == null)
            {
                return HttpNotFound();
            }
            return View(blogPost);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            var dateTimeStamp = $"{DateTime.Now.ToString("yyyyMddHHmmss")}";
            BlogPost defaultBlogPost = new BlogPost
            {
                Title = "Test Title " + dateTimeStamp,
                BlogPostBody = "Test Body " + dateTimeStamp,
                Abstract = "My Abstract " + dateTimeStamp,
                Published = true
            };
            return View(defaultBlogPost);
        }

        // POST: BlogPosts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles="Admin")]
        public ActionResult Create([Bind(Include = "Title,Abstract,BlogPostBody,Published")] BlogPost blogPost, HttpPostedFileBase imageFile)
        {
            if (string.IsNullOrWhiteSpace(blogPost.Title))
            {
                ModelState.AddModelError("Invalid Title", "You must provide a Blog Title.");
                return View(blogPost);
            }
            blogPost.Slug = blogPost.Title.GenerateSlug();
            if (String.IsNullOrWhiteSpace(blogPost.Slug))
            {
                ModelState.AddModelError("Title", @"Invalid Title (results in empty surrogate key)");
                return View(blogPost);
            }
            if (db.BlogPosts.Any(p => p.Slug == blogPost.Slug))
            {
                ModelState.AddModelError("Title", @"Invalid Title (unable to create unique surrogate key)");
                return View(blogPost);
            }
            if (ModelState.IsValid)
            {
                if (HelperMethods.IsWebFriendlyImage(imageFile))
                {
                    // Run filename through URL Friendly method then Apply a timestamp to filename to avoid naming collisions
                    var fileName = imageFile.FileName.GenerateSlug();
                    var massagedFileName = $"{Path.GetFileNameWithoutExtension(fileName)}_{DateTime.Now.Ticks}{Path.GetExtension(fileName)}";
                    var dirPath = Server.MapPath("~/Uploads/");
                    if (HelperMethods.EnsureDirectoryExists(dirPath))
                    {
                        var filePath = Path.Combine(dirPath, massagedFileName);
                        imageFile.SaveAs(filePath);
                        blogPost.ImagePath = $"/Uploads/{massagedFileName}";
                    }
                }
                blogPost.Created = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

                db.BlogPosts.Add(blogPost);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(blogPost);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Edit(string slug)
        {
            if (slug == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BlogPost blogPost = db.BlogPosts.FirstOrDefault(b => b.Slug == slug);
            if (blogPost == null)
            {
                return HttpNotFound();
            }
            return View(blogPost);
        }

        // POST: BlogPosts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit([Bind(Include = "Id,Created,Title,Abstract,BlogPostBody,ImagePath,Published,Slug")] BlogPost blogPost, string foo)
        {
            if (ModelState.IsValid)
            {
                //blogPost.Updated = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                blogPost.Updated = DateTime.Now;
                db.Entry(blogPost).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(blogPost);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BlogPost blogPost = db.BlogPosts.Find(id);
            db.BlogPosts.Remove(blogPost);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // POST: BlogPosts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            // TODO: DELETE THIS Action
            BlogPost blogPost = db.BlogPosts.Find(id);
            db.BlogPosts.Remove(blogPost);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
