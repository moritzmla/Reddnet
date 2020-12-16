using Microsoft.AspNetCore.Http;
using Reddnet.Application.Interfaces;
using Reddnet.Web.Extensions;
using System;

namespace Reddnet.Web.Authorization
{
    class CurrentUserAccessor : ICurrentUserAccessor
    {
        private readonly IHttpContextAccessor contextAccessor;

        public CurrentUserAccessor(IHttpContextAccessor contextAccessor)
            => this.contextAccessor = contextAccessor;

        public Guid Id => contextAccessor.HttpContext.User.GetUserId();
        public string UserName => contextAccessor.HttpContext.User.Identity.Name;
        public bool IsAuthenticated => contextAccessor.HttpContext.User.Identity.IsAuthenticated;
    }
}
