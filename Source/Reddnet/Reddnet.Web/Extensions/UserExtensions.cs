using System;
using System.Security.Claims;

namespace Reddnet.Web.Extensions
{
    public static class UserExtensions
    {
        public static Guid GetUserId(this ClaimsPrincipal claimsPrincipal)
            => Guid.Parse(claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier));
    }
}
