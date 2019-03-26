using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
    public class HomeController : Controller
    {
        protected ApplicationDbContext applicationContext;
        protected UserManager<ApplicationUser> userManager;
        protected SignInManager<ApplicationUser> signInManager;
        protected RoleManager<IdentityRole> roleManager;

        public HomeController(ApplicationDbContext applicationContext, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            this.applicationContext = applicationContext;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
        }

        #region Index

        public IActionResult Index()
        {
            SearchViewModel search = new SearchViewModel
            {
                Blogs = AutoInclude.IncludeAll<BlogDataModel>(this.applicationContext.Blogs),
                Posts = AutoInclude.IncludeAll<PostDataModel>(this.applicationContext.Posts)
            };
            return View(search);
        }

        #endregion

        #region Search

        [HttpPost]
        public IActionResult Search(string searchString)
        {
            if(string.IsNullOrWhiteSpace(searchString))
            {
                return RedirectToAction("Index", "Home");
            }

            var posts = AutoInclude.IncludeAll<PostDataModel>(this.applicationContext.Posts);
            var blogs = AutoInclude.IncludeAll<BlogDataModel>(this.applicationContext.Blogs);
            var users = this.applicationContext.Users
                .Include(x => x.Posts)
                .ThenInclude(x => x.Comments)
                .Include(x => x.Posts)
                .ThenInclude(x => x.Author)
                .Include(x => x.Posts)
                .ThenInclude(x => x.Blog)
                .Include(x => x.Comments).ToList();

            List<PostDataModel> searchedPost = new List<PostDataModel>();
            foreach (PostDataModel post in posts)
            {
                if(post.Title.ToLower().Contains(searchString.ToLower()) || post.Preview.ToLower().Contains(searchString.ToLower()) || post.Tags.ToLower().Contains(searchString.ToLower()))
                {
                    if(!searchedPost.Contains(post))
                    {
                        searchedPost.Add(post);
                    }
                }
            }

            List<BlogDataModel> searchedBlogs = new List<BlogDataModel>();
            foreach (BlogDataModel blog in blogs)
            {
                if (blog.Name.ToLower().Contains(searchString.ToLower()) || blog.Description.ToLower().Contains(searchString.ToLower()))
                {
                    if (!searchedBlogs.Contains(blog))
                    {
                        searchedBlogs.Add(blog);
                    }
                }
            }

            List<ApplicationUser> searchedUsers = new List<ApplicationUser>();
            foreach (ApplicationUser user in users)
            {
                if (user.UserName.ToLower().Contains(searchString.ToLower()))
                {
                    if (!searchedUsers.Contains(user))
                    {
                        searchedUsers.Add(user);
                    }
                }
            }

            SearchViewModel result = new SearchViewModel
            {
                Posts = searchedPost,
                Blogs = searchedBlogs,
                Users = searchedUsers
            };

            return View(result);
        }

        #endregion

        #region NoAccess

        public IActionResult NoAccess()
        {
            return View();
        }

        #endregion

        #region AdminPanel

        [Authorize(Roles = "Administrator")]
        public IActionResult AdminPanel()
        {
            return View();
        }

        [Authorize(Roles = "Administrator")]
        public IActionResult Users()
        {
            return View(applicationContext.Users.ToList());
        }

        [Authorize(Roles = "Administrator")]
        public IActionResult Blogs()
        {
            return View(applicationContext.Blogs.ToList());
        }

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> SetAdmin(string id)
        {
            await this.userManager.AddToRoleAsync(await userManager.FindByIdAsync(id), "Administrator");
            this.applicationContext.SaveChanges();

            return RedirectToAction("Users");
        }

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            await this.userManager.DeleteAsync(await userManager.FindByIdAsync(id));
            this.applicationContext.SaveChanges();

            return RedirectToAction("Users");
        }

        #endregion

        #region Settings

        [Authorize(Roles = "Administrator")]
        public IActionResult Settings()
        {
            SettingDataModel settingDataModel = applicationContext.Settings.FirstOrDefault(o => o.Id == 1);

            SettingViewModel settingViewModel = new SettingViewModel();
            settingViewModel.Title = settingDataModel.Title;

            ViewBag.LogoToBytes = settingDataModel.Logo;

            return View(settingViewModel);
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Settings(SettingViewModel settingViewModel)
        {
            SettingDataModel settingDataModel = applicationContext.Settings.FirstOrDefault(o => o.Id == 1);

            if (ModelState.IsValid)
            {
                if(!(settingViewModel.Logo == null || settingViewModel.Logo.Length <= 0))
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        settingViewModel.Logo.CopyTo(memoryStream);
                        settingDataModel.Logo = memoryStream.ToArray();
                    }
                }

                if (!(settingViewModel.Background == null || settingViewModel.Background.Length <= 0))
                {
                    System.IO.File.Delete(".//wwwroot//images//Background.png");
                    using (var fileStream = new FileStream(".//wwwroot//images//Background.png", FileMode.Create))
                    {
                        settingViewModel.Background.CopyTo(fileStream);
                    }
                }

                settingDataModel.Title = settingViewModel.Title;

                this.applicationContext.Update(settingDataModel);
                this.applicationContext.SaveChanges();
            }

            ViewBag.LogoToBytes = settingDataModel.Logo;

            return View(settingViewModel);
        }

        #endregion

        #region Methods

        #endregion
    }
}