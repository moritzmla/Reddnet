using Reddnet.Application.Interfaces;
using Reddnet.Web.Extensions;

namespace Reddnet.Web.Authorization;

class User : IUser
{
    private readonly IHttpContextAccessor contextAccessor;

    public User(IHttpContextAccessor contextAccessor)
        => this.contextAccessor = contextAccessor;

    public Guid Id => contextAccessor.HttpContext.User.GetUserId();
    public string UserName => contextAccessor.HttpContext.User.Identity.Name;
    public bool IsAuthenticated => contextAccessor.HttpContext.User.Identity.IsAuthenticated;
}
