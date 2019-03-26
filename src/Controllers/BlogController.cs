using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Reddnet.Data.AccountData;
using Reddnet.Data.ApplicationData;
using Reddnet.Models.DataModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Reddnet.Controllers
{
    public class BlogController : Controller
    {
        protected ApplicationDbContext applicationContext;
        protected UserManager<ApplicationUser> userManager;
        protected SignInManager<ApplicationUser> signInManager;
        protected RoleManager<IdentityRole> roleManager;

        public BlogController(ApplicationDbContext applicationContext, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            this.applicationContext = applicationContext;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
        }

        #region View

        public IActionResult View(int id)
        {
            var blog = this.applicationContext.Blogs
                .Include(x => x.Posts)
                .ThenInclude(x => x.Comments)
                .Include(x => x.Posts)
                .ThenInclude(x => x.Author)
                .FirstOrDefault(x => x.BlogId == id);
            return View(blog);
        }

        #endregion

        #region New

        [Authorize(Roles = "Administrator")]
        public IActionResult New()
        {
            return View();
        }

        [Authorize(Roles = "Administrator"), HttpPost, ValidateAntiForgeryToken]
        public IActionResult New(BlogDataModel blog, IFormFile formFile)
        {
            if (ModelState.IsValid)
            {
                if (formFile == null || formFile.Length <= 0)
                {
                    ModelState.AddModelError("", "Your Blog need a Cover!");
                    return View(blog);
                }

                BlogDataModel newBlog = new BlogDataModel
                {
                    Name = blog.Name,
                    Description = blog.Description,
                    Cover = FileToByteArray(formFile),
                    Created = DateTime.Now
                };

                this.applicationContext.Blogs.Add(newBlog);
                this.applicationContext.SaveChanges();

                return RedirectToAction("View", "Blog", new { id = this.applicationContext.Blogs.FirstOrDefault(x => x.Name == newBlog.Name).BlogId });
            }
            return View(blog);
        }

        #endregion

        #region Edit

        [Authorize(Roles = "Administrator")]
        public IActionResult Edit(int id)
        {
            return View(this.applicationContext.Blogs.FirstOrDefault(x => x.BlogId == id));
        }

        [Authorize(Roles = "Administrator"), HttpPost, ValidateAntiForgeryToken]
        public IActionResult Edit(int id, BlogDataModel blog, IFormFile formFile)
        {
            BlogDataModel target = this.applicationContext.Blogs.FirstOrDefault(x => x.BlogId == id);

            if (ModelState.IsValid)
            {
                target.Name = blog.Name;
                target.Description = blog.Description;
                if (!(formFile == null || formFile.Length <= 0))
                {
                    target.Cover = FileToByteArray(formFile);
                }

                this.applicationContext.Blogs.Update(target);
                this.applicationContext.SaveChanges();

                return RedirectToAction("View", "Blog", new { id });
            }

            blog.Cover = target.Cover;
            return View(blog);
        }

        #endregion

        #region Delete

        [Authorize(Roles = "Administrator")]
        public IActionResult Delete(int id)
        {
            BlogDataModel blog = this.applicationContext.Blogs.Include(x => x.Posts).FirstOrDefault(x => x.BlogId == id);
            this.applicationContext.Posts.RemoveRange(blog.Posts);
            this.applicationContext.Blogs.Remove(blog);
            this.applicationContext.SaveChanges();

            return RedirectToAction("Index", "Home");
        }

        #endregion

        #region Methods

        private byte[] FileToByteArray(IFormFile _formFile)
        {
            using (MemoryStream _memoryStream = new MemoryStream())
            {
                _formFile.CopyTo(_memoryStream);
                return _memoryStream.ToArray();
            }
        }

        #endregion
    }
}