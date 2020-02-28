using Reddnet.Core.Entities;
using System.Collections.Generic;

namespace Reddnet.ViewModels
{
    public class SearchViewModel
    {
        public IReadOnlyList<PostEntity> Posts { get; set; }

        public IReadOnlyList<BlogEntity> Blogs { get; set; }

        public IReadOnlyList<AuthorEntity> Users { get; set; }
    }
}
