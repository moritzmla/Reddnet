using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Reddnet.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace Reddnet.Web.Pages.Account;

public class LoginModel : PageModel
{
    private UserManager<UserEntity> userManager;
    private SignInManager<UserEntity> signInManager;

    public LoginModel(SignInManager<UserEntity> signInManager, UserManager<UserEntity> userManager)
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
            var user = await this.userManager.FindByEmailAsync(this.Email);

            if (user != null)
            {
                var result = await this.signInManager.PasswordSignInAsync(user, this.Password, true, false);

                if (result.Succeeded)
                {
                    return Redirect(string.IsNullOrEmpty(this.ReturnUrl) ? RouteConstants.Index : this.ReturnUrl);
                }
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
    [DataType(DataType.Password)]
    [BindProperty]
    public string Password { get; set; }
}
