using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Reddnet.Data;
using Reddnet.Data.AccountData;
using Reddnet.Data.ApplicationData;
using Reddnet.Models.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;

namespace Reddnet.Controllers
{
    public class AccountController : Controller
    {
        protected readonly ApplicationDbContext applicationContext;

        protected UserManager<ApplicationUser> userManager;
        protected SignInManager<ApplicationUser> signInManager;

        public AccountController(ApplicationDbContext applicationContext, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            this.applicationContext = applicationContext;
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        #region Settings

        [Authorize]
        public IActionResult Settings(string id)
        {
            if (id != this.User.Identity.Name)
            {
                return RedirectToAction("NoAccess", "Home");
            }

            ApplicationUser target = this.userManager.Users.FirstOrDefault(u => u.UserName == id);
            ViewBag.CurrentUserPicture = target.Image;

            return View();
        }

        [Authorize]
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Settings(string id, ProfilViewModel profilViewModel)
        {
            ApplicationUser target = this.userManager.Users.FirstOrDefault(u => u.UserName == id);

            if (ModelState.IsValid)
            {
                if (!(profilViewModel.ProfilPicture == null || profilViewModel.ProfilPicture.Length <= 0))
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        profilViewModel.ProfilPicture.CopyTo(memoryStream);
                        target.Image = memoryStream.ToArray();
                    }
                }

                await this.userManager.UpdateAsync(target);
                return RedirectToAction("Profil", "Account", new { id });
            }

            ViewBag.CurrentUserPicture = target.Image;

            return View(profilViewModel);
        }

        #endregion

        #region Profil

        public IActionResult Profil(string id)
        {
            ApplicationUser profil = this.applicationContext.Users
                .Include(x => x.Posts)
                .ThenInclude(x => x.Comments)
                .Include(x => x.Posts)
                .ThenInclude(x => x.Author)
                .Include(x => x.Posts)
                .ThenInclude(x => x.Blog)
                .Include(x => x.Comments)
                .Include(x => x.Posts)
                .FirstOrDefault(u => u.UserName == id);
            return View(profil);
        }

        #endregion

        #region Register

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Register(RegisterViewModel registerViewModel)
        {
            if (ModelState.IsValid)
            {
                if(registerViewModel.Password.Equals(registerViewModel.ConfirmPassword))
                {
                    ApplicationUser applicationUser = new ApplicationUser();
                    applicationUser.UserName = registerViewModel.UserName;
                    applicationUser.Email = registerViewModel.Email;
                    applicationUser.Image = System.IO.File.ReadAllBytes(".//wwwroot//images//ProfilPicture.png");

                    IdentityResult result = userManager.CreateAsync(applicationUser, registerViewModel.Password).Result;

                    if(result.Succeeded)
                    {
                        this.userManager.AddToRoleAsync(applicationUser, "Writer");

                        this.signInManager.PasswordSignInAsync(registerViewModel.UserName, registerViewModel.Password, registerViewModel.RememberMe, false);

                        return RedirectToAction("Index", "Home");
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
        public IActionResult Login(LoginViewModel loginViewModel)
        {
            if (ModelState.IsValid)
            {
                string userName = loginViewModel.UserName;
                if (userName.IndexOf("@") > -1)
                {
                    var user = this.userManager.FindByEmailAsync(userName);

                    if (user == null)
                    {
                        ModelState.AddModelError("", "Invalid login attempt.");
                        return View(loginViewModel);
                    } else
                    {
                        userName = user.Result.UserName;
                    }
                }
                var result = signInManager.PasswordSignInAsync(userName, loginViewModel.Password, loginViewModel.RememberMe, false).Result;
                if(result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
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
        [Route("Logout")]
        public async Task<IActionResult> LogOutAsync()
        {
            await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
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