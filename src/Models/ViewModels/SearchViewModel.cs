using Reddnet.Data.AccountData;
using Reddnet.Models.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Reddnet.Models.ViewModels
{
    public class SearchViewModel
    {
        public IEnumerable<PostDataModel> Posts { get; set; }

        public IEnumerable<BlogDataModel> Blogs { get; set; }

        public IEnumerable<ApplicationUser> Users { get; set; }
    }
}
