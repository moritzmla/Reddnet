using BlogCoreEngine.Core.Entities;
using System.Collections.Generic;

namespace BlogCoreEngine.ViewModels
{
    public class SearchViewModel
    {
        public IReadOnlyList<PostDataModel> Posts { get; set; }

        public IReadOnlyList<BlogDataModel> Blogs { get; set; }

        public IReadOnlyList<Author> Users { get; set; }
    }
}
