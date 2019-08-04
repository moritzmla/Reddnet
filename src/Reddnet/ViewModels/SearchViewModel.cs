using BlogCoreEngine.Core.Entities;
using BlogCoreEngine.DataAccess.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogCoreEngine.ViewModels
{
    public class SearchViewModel
    {
        public IReadOnlyList<PostDataModel> Posts { get; set; }

        public IReadOnlyList<BlogDataModel> Blogs { get; set; }

        public IReadOnlyList<Author> Users { get; set; }
    }
}
