using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Reddnet.Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Reddnet.Web.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly UserManager<UserEntity> userManager;
        private readonly SignInManager<UserEntity> signInManager;

        public RegisterModel(SignInManager<UserEntity> signInManager, UserManager<UserEntity> userManager)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
        }

        public void OnGet(string returnUrl = default)
        {
            this.ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPost()
        {
            if (ModelState.IsValid)
            {
                var user = new UserEntity
                {
                    UserName = this.UserName,
                    Email = this.Email
                };

                var result = await this.userManager.CreateAsync(user, this.Password);

                if (result.Succeeded)
                {
                    await this.signInManager.SignInAsync(user, true);
                    return Redirect(string.IsNullOrEmpty(this.ReturnUrl) ? "/" : this.ReturnUrl);
                }
            }

            return Page();
        }

        public string ReturnUrl { get; set; }
        [Required]
        [EmailAddress]
        [BindProperty]
        public string Email { get; set; }
        [Required]
        [BindProperty]
        public string UserName { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [BindProperty]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [BindProperty]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }
    }
}