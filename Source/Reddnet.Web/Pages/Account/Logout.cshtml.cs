using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Reddnet.Domain.Entities;

namespace Reddnet.Web.Pages.Account;

[Authorize]
public class LogoutModel : PageModel
{
    private readonly SignInManager<UserEntity> signInManager;

    public LogoutModel(SignInManager<UserEntity> signInManager) => this.signInManager = signInManager;

    public async Task<IActionResult> OnGet()
    {
        await this.signInManager.SignOutAsync();
        return Redirect(RouteConstants.Index);
    }
}
