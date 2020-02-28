using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Reddnet.Core.Entities;
using Reddnet.Core.Interfaces;
using Reddnet.DataAcces.Extensions;
using Reddnet.ViewModels;
using System;
using System.Linq;
using System.Threading.Tasks;
using Reddnet.DataAccess.Identity;
using Reddnet.DataAcces.Identity;

namespace Reddnet.Controllers
{
    public class HomeController : Controller
    {
        private readonly IAsyncRepository<BlogEntity> blogRepository;
        private readonly IAsyncRepository<PostEntity> postRepository;
        private readonly IAsyncRepository<AuthorEntity> AuthorEntityRepository;

        private readonly UserManager<ApplicationUser> userManager;

        public HomeController(
            IAsyncRepository<BlogEntity> blogRepository,
            IAsyncRepository<PostEntity> postRepository,
            IAsyncRepository<AuthorEntity> AuthorEntityRepository,
            UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
            this.AuthorEntityRepository = AuthorEntityRepository;
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
            if (string.IsNullOrWhiteSpace(searchString))
            {
                return this.RedirectToAsync<HomeController>(x => x.Index());
            }

            var posts = await this.postRepository.GetAll();
            var blogs = await this.blogRepository.GetAll();
            var users = await this.AuthorEntityRepository.GetAll();

            var searchedPost = posts.Where(
                x => x.Title.Contains(searchString, StringComparison.OrdinalIgnoreCase) ||
                x.Content.Contains(searchString, StringComparison.OrdinalIgnoreCase));

            var searchedBlogs = blogs.Where(
                x => x.Name.Contains(searchString, StringComparison.OrdinalIgnoreCase) ||
                x.Description.Contains(searchString, StringComparison.OrdinalIgnoreCase));

            var searchedUsers = users.Where(
                x => x.Name.Contains(searchString, StringComparison.OrdinalIgnoreCase));

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

        [Authorize(Roles = ApplicationRoles.Administrator)]
        public IActionResult AdminPanel()
        {
            return View();
        }

        [Authorize(Roles = ApplicationRoles.Administrator)]
        public IActionResult Users()
        {
            return View(this.userManager.Users.ToList());
        }

        [Authorize(Roles = ApplicationRoles.Administrator)]
        public async Task<IActionResult> Blogs()
        {
            return View(await this.blogRepository.GetAll());
        }

        [Authorize(Roles = ApplicationRoles.Administrator)]
        public async Task<IActionResult> SetAdmin(string id)
        {
            await this.userManager.AddToRoleAsync(await userManager.FindByIdAsync(id), ApplicationRoles.Administrator);
            return this.RedirectTo<HomeController>(x => x.Users());
        }

        [Authorize(Roles = ApplicationRoles.Administrator)]
        public async Task<IActionResult> DeleteUser(string id)
        {
            await this.userManager.DeleteAsync(await userManager.FindByIdAsync(id));
            return this.RedirectTo<HomeController>(x => x.Users());
        }

        #endregion
    }
}