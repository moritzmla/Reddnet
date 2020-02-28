using System;
using System.Security.Claims;
using System.Security.Principal;

namespace Reddnet.DataAccess.Extensions
{
    public static class IdentityExtensions
    {
        public static Guid GetAuthorId(this IIdentity identity)
        {
            var AuthorId = (identity as ClaimsIdentity)?.FindFirst("AuthorId");
            return (AuthorId != null) ? Guid.Parse(AuthorId.Value) : Guid.Empty;
        }

        public static string GetAuthorEntityName(this IIdentity identity)
        {
            var AuthorEntityName = (identity as ClaimsIdentity)?.FindFirst("AuthorName");
            return (AuthorEntityName != null) ? AuthorEntityName.Value : string.Empty;
        }
    }
}
