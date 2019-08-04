using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BlogCoreEngine.Core.Entities;
using BlogCoreEngine.Core.Interfaces;
using BlogCoreEngine.DataAccess.Data;
using BlogCoreEngine.ViewModels;
using BlogCoreEngine.Web.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BlogCoreEngine.Controllers
{
    public class HomeController : Controller
    {
        private readonly IAsyncRepository<BlogDataModel> blogRepository;
        private readonly IAsyncRepository<PostDataModel> postRepository;
        private readonly IAsyncRepository<Author> authorRepository;
        private readonly IBlogOptionService blogOptionService;

        private readonly UserManager<ApplicationUser> userManager;

        public HomeController(
            IAsyncRepository<BlogDataModel> blogRepository, 
            IAsyncRepository<PostDataModel> postRepository,
            IAsyncRepository<Author> authorRepository,
            IBlogOptionService blogOptionService,
            UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
            this.blogOptionService = blogOptionService;
            this.authorRepository = authorRepository;
            this.blogRepository = blogRepository;
            this.postRepository = postRepository;
        }

        #region Index

        public async Task<IActionResult> Index()
        {
            SearchViewModel search = new SearchViewModel
            {
                Blogs = await this.blogRepository.GetAll(),
                Posts = await this.postRepository.GetAll()
            };

            return View(search);
        }

        #endregion

        #region Search

        [HttpPost]
        public async Task<IActionResult> Search(string searchString)
        {
            if(string.IsNullOrWhiteSpace(searchString))
            {
                return RedirectToAction("Index", "Home");
            }

            var posts = await this.postRepository.GetAll();
            var blogs = await this.blogRepository.GetAll();
            var users = await this.authorRepository.GetAll();

            var searchedPost = posts.Where(
                x => x.Title.ToLower().Contains(searchString.ToLower()) ||
                x.Content.ToLower().Contains(searchString.ToLower()));

            var searchedBlogs = blogs.Where(
                x => x.Name.ToLower().Contains(searchString.ToLower()) ||
                x.Description.ToLower().Contains(searchString.ToLower()));

            var searchedUsers = users.Where(
                x => x.Name.ToLower().Contains(searchString.ToLower()));

            SearchViewModel result = new SearchViewModel
            {
                Posts = searchedPost.ToList(),
                Blogs = searchedBlogs.ToList(),
                Users = searchedUsers.ToList()
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
            return View(this.userManager.Users.ToList());
        }

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Blogs()
        {
            return View(await this.blogRepository.GetAll());
        }

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> SetAdmin(string id)
        {
            await this.userManager.AddToRoleAsync(await userManager.FindByIdAsync(id), "Administrator");
            return RedirectToAction("Users");
        }

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            await this.userManager.DeleteAsync(await userManager.FindByIdAsync(id));
            return RedirectToAction("Users");
        }

        #endregion

        #region Settings

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Settings()
        {
            SettingViewModel settingViewModel = new SettingViewModel();

            settingViewModel.Title = await this.blogOptionService.GetTitle();
            ViewBag.LogoToBytes = await this.blogOptionService.GetLogo();

            return View(settingViewModel);
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Settings(SettingViewModel settingViewModel)
        {
            if (ModelState.IsValid)
            {
                if(!(settingViewModel.Logo == null || settingViewModel.Logo.Length <= 0))
                {
                    await this.blogOptionService.SetLogo(settingViewModel.Logo.ToByteArray());
                }

                await this.blogOptionService.SetTitle(settingViewModel.Title);
            }

            ViewBag.LogoToBytes = await this.blogOptionService.GetLogo();

            return View(settingViewModel);
        }

        #endregion
    }
}