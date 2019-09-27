using System;
using System.Linq;
using System.Threading.Tasks;
using BlogCoreEngine.Core.Entities;
using BlogCoreEngine.Core.Interfaces;
using BlogCoreEngine.DataAccess.Data;
using BlogCoreEngine.DataAccess.Extensions;
using BlogCoreEngine.ViewModels;
using BlogCoreEngine.Web.Extensions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Reddnet.Web.Extensions;

namespace BlogCoreEngine.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAsyncRepository<Author> authorRepository;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;

        public AccountController(IAsyncRepository<Author> authorRepository, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            this.authorRepository = authorRepository;
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        #region Options

        [Authorize]
        public IActionResult Settings(string id)
        {
            if (id != this.User.Identity.GetAuthorName())
            {
                return this.RedirectTo<HomeController>(x => x.NoAccess());
            }

            var user = this.userManager.Users.FirstOrDefault(u => u.UserName == id);
            ViewBag.CurrentUserPicture = user.Author.Image;

            return View();
        }

        [Authorize, HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Settings(string id, ProfileViewModel profileViewModel)
        {
            var user = await this.userManager.Users.FirstOrDefaultAsync(u => u.UserName == id);

            if (ModelState.IsValid)
            {
                if (!(profileViewModel.ProfilePicture == null || profileViewModel.ProfilePicture.Length <= 0))
                {
                    user.Author.Image = profileViewModel.ProfilePicture.ToByteArray();
                }

                await this.userManager.UpdateAsync(user);
                return this.RedirectToAsync<AccountController>(x => x.Profile(id));
            }

            ViewBag.CurrentUserPicture = user.Author.Image;
            return View(profileViewModel);
        }

        #endregion

        #region Profile

        public async Task<IActionResult> Profile(string id)
        {
            var user = await this.userManager.FindByNameAsync(id);
            return View(user.Author);
        }

        #endregion

        #region Register

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            if (ModelState.IsValid)
            {
                if(registerViewModel.Password.Equals(registerViewModel.ConfirmPassword))
                {
                    var author = await this.authorRepository.Add(new Author
                    {
                        Id = Guid.NewGuid(),
                        Created = DateTime.Now,
                        Modified = DateTime.Now,
                        Name = registerViewModel.UserName,
                        Image = System.IO.File.ReadAllBytes(".//wwwroot//images//Profile.png")
                    });

                    var applicationUser = new ApplicationUser
                    {
                        UserName = registerViewModel.UserName,
                        Email = registerViewModel.Email,
                        Author = author
                    };

                    var result = await userManager.CreateAsync(applicationUser, registerViewModel.Password);

                    if(result.Succeeded)
                    {
                        await this.signInManager.PasswordSignInAsync(
                            registerViewModel.UserName, 
                            registerViewModel.Password, 
                            registerViewModel.RememberMe, 
                            false);

                        return this.RedirectToAsync<HomeController>(x => x.Index());
                    } else
                    {
                        ModelState.AddModelError("", "Something dosen´t work.");
                    }
                } else
                {
                    ModelState.AddModelError("", "Passwords dosen´t are the same.");
                }
            }

            return View(registerViewModel);
        }

        #endregion

        #region Login

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if (ModelState.IsValid)
            {
                string userName = loginViewModel.UserName;

                if (userName.IndexOf("@") > -1)
                {
                    var user = await this.userManager.FindByEmailAsync(userName);

                    if (user == null)
                    {
                        ModelState.AddModelError("", "Invalid login attempt.");
                        return View(loginViewModel);
                    } else
                    {
                        userName = user.UserName;
                    }
                }

                var result = await signInManager.PasswordSignInAsync(userName, loginViewModel.Password, loginViewModel.RememberMe, false);

                if(result.Succeeded)
                {
                    return this.RedirectToAsync<HomeController>(x => x.Index());
                }
                else
                {
                    ModelState.AddModelError("", "Password or Username is not right.");
                }
            }

            return View(loginViewModel);
        }

        #endregion

        #region LogOut

        [Authorize]
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
            return this.RedirectToAsync<HomeController>(x => x.Index());
        }

        #endregion
    }
}