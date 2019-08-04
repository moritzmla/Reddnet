using BlogCoreEngine.DataAccess.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BlogCoreEngine.DataAccess.Extensions
{
    public class AppClaimsPrincipalFactory : UserClaimsPrincipalFactory<ApplicationUser, IdentityRole>
    {
        public AppClaimsPrincipalFactory(
            UserManager<ApplicationUser> userManager
            , RoleManager<IdentityRole> roleManager
            , IOptions<IdentityOptions> optionsAccessor) : base(userManager, roleManager, optionsAccessor) { }

        public async override Task<ClaimsPrincipal> CreateAsync(ApplicationUser user)
        {
            var principal = await base.CreateAsync(user);

            if (user.AuthorId != null)
            {
                (principal.Identity as ClaimsIdentity).AddClaims(new[] {
                    new Claim("AuthorId", user.AuthorId.ToString()),
                    new Claim("AuthorName", user.Author.Name)
                });
            }

            return principal;
        }
    }
}
