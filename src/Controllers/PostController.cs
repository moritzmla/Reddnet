using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Reddnet.Data;
using Reddnet.Data.AccountData;
using Reddnet.Data.ApplicationData;
using Reddnet.Models.DataModels;
using Reddnet.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;

namespace Reddnet.Controllers
{
    public class PostController : Controller
    {
        protected ApplicationDbContext applicationContext;
        protected UserManager<ApplicationUser> userManager;
        protected SignInManager<ApplicationUser> signInManager;
        protected RoleManager<IdentityRole> roleManager;

        public PostController(ApplicationDbContext applicationContext, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            this.applicationContext = applicationContext;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
        }

        #region Pin

        [Authorize(Roles = "Administrator")]
        public IActionResult Pin(int id)
        {
            var post = AutoInclude.IncludeAll<PostDataModel>(this.applicationContext.Posts).FirstOrDefault(p => p.PostId == id);

            if (post.Pinned)
            {
                post.Pinned = false;
            }
            else
            {
                post.Pinned = true;
            }

            this.applicationContext.Posts.Update(post);
            this.applicationContext.SaveChanges();

            return RedirectToAction("View", "Blog", new { id = post.Blog.BlogId });
        }

        #endregion

        #region Details

        public IActionResult Details(int id)
        {
            PostDataModel post = this.applicationContext.Posts
                .Include(c => c.Comments)
                .ThenInclude(sn => sn.Author)
                .ThenInclude(sn => sn.Posts)
                .Include(c => c.Comments)
                .ThenInclude(sn => sn.Post)
                .Include(a => a.Author)
                .Include(x => x.Blog)
                .FirstOrDefault(p => p.PostId == id);

            if (post != null)
            {
                post.Views += 1;
                this.applicationContext.Posts.Update(post);
                this.applicationContext.SaveChanges();
            }

            return View(post);
        }

        #endregion

        #region Edit

        [Authorize]
        public IActionResult Edit(int id)
        {
            PostDataModel target = AutoInclude.IncludeAll<PostDataModel>(this.applicationContext.Posts).FirstOrDefault(bp => bp.PostId == id);

            if (!(this.User.FindFirstValue(ClaimTypes.NameIdentifier).Equals(target.Author.Id) || this.User.IsInRole("Administrator")))
            {
                return RedirectToAction("NoAccess", "Home");
            }

            return View(target);
        }

        [Authorize]
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Edit(int id, PostDataModel postDataModel, IFormFile _formFile)
        {
            PostDataModel post = AutoInclude.IncludeAll<PostDataModel>(this.applicationContext.Posts).FirstOrDefault(bp => bp.PostId == id);

            if (ModelState.IsValid)
            {
                if (post != null)
                {
                    post.Title = postDataModel.Title;
                    post.Preview = postDataModel.Preview;
                    post.Content = postDataModel.Content;
                    post.LastChangeDate = DateTime.Now;
                    post.Link = postDataModel.Link;
                    post.Tags = postDataModel.Tags;

                    if (_formFile != null)
                    {
                        post.Cover = FileToByteArray(_formFile);
                    }
                }

                this.applicationContext.Posts.Update(post);
                this.applicationContext.SaveChanges();

                return RedirectToAction("View", "Blog", new { id = post.Blog.BlogId });
            }

            postDataModel.Title = post.Title;
            postDataModel.Content = post.Content;
            postDataModel.Cover = post.Cover;

            return View(postDataModel);
        }

        #endregion

        #region New

        [Authorize]
        public IActionResult New(int id)
        {
            return View();
        }

        [Authorize]
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult New(int id, PostDataModel postDataModel, IFormFile _formFile)
        {
            if (ModelState.IsValid)
            {
                if (_formFile != null)
                {
                    postDataModel.Cover = FileToByteArray(_formFile);
                }

                postDataModel.Blog = this.applicationContext.Blogs.FirstOrDefault(x => x.BlogId == id);

                ApplicationUser currentUser = this.applicationContext.Users.FirstOrDefault(u => u.Id == this.User.FindFirstValue(ClaimTypes.NameIdentifier));

                currentUser.Posts.Add(postDataModel);
                this.applicationContext.Posts.Add(postDataModel);

                this.applicationContext.Users.Update(currentUser);
                this.applicationContext.SaveChanges();

                return RedirectToAction("View", "Blog", new { id });
            }

            return View(postDataModel);
        }

        #endregion

        #region Archiv

        [Authorize]
        public async Task<IActionResult> Archiv(int id)
        {
            PostDataModel target = AutoInclude.IncludeAll<PostDataModel>(this.applicationContext.Posts).FirstOrDefault(bp => bp.PostId == id);

            if (!(this.User.FindFirstValue(ClaimTypes.NameIdentifier).Equals(target.Author.Id) || this.User.IsInRole("Administrator")))
            {
                return RedirectToAction("NoAccess", "Home");
            }

            if (target.Archieved == false)
            {
                target.Archieved = true;
            }
            else
            {
                target.Archieved = false;
            }

            this.applicationContext.Posts.Update(target);
            await this.applicationContext.SaveChangesAsync();

            return RedirectToAction("Details", "Post", new { id });
        }

        #endregion

        #region Delete

        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            PostDataModel target = AutoInclude.IncludeAll<PostDataModel>(this.applicationContext.Posts).FirstOrDefault(bp => bp.PostId == id);
            ApplicationUser author = this.applicationContext.Users.FirstOrDefault(u => u.Id == target.Author.Id);

            if (!(this.User.FindFirstValue(ClaimTypes.NameIdentifier).Equals(target.Author.Id) || this.User.IsInRole("Administrator")))
            {
                return RedirectToAction("NoAccess", "Home");
            }

            author.Posts.Remove(target);

            this.applicationContext.Comments.RemoveRange(target.Comments);

            if (target != null)
            {
                this.applicationContext.Users.Update(author);
                this.applicationContext.Posts.Remove(target);
            }

            await this.applicationContext.SaveChangesAsync();
            return RedirectToAction("View", "Blog", new { id = target.Blog.BlogId });
        }

        #endregion

        #region Methods

        private byte[] FileToByteArray(IFormFile _formFile)
        {
            using(MemoryStream _memoryStream = new MemoryStream())
            {
                _formFile.CopyTo(_memoryStream);
                return _memoryStream.ToArray();
            }
        }

        #endregion
    }
}