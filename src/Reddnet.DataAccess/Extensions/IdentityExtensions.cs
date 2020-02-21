using System;
using System.Security.Claims;
using System.Security.Principal;

namespace BlogCoreEngine.DataAccess.Extensions
{
    public static class IdentityExtensions
    {
        public static Guid GetAuthorId(this IIdentity identity)
        {
            var authorId = (identity as ClaimsIdentity).FindFirst("AuthorId");
            return (authorId != null) ? Guid.Parse(authorId.Value) : Guid.Empty;
        }

        public static string GetAuthorName(this IIdentity identity)
        {
            var authorName = (identity as ClaimsIdentity).FindFirst("AuthorName");
            return (authorName != null) ? authorName.Value : string.Empty;
        }
    }
}
