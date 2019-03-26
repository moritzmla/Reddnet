using Reddnet.Models.DataModels;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Reddnet.Data.AccountData
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public byte[] Image { get; set; }

        public virtual ICollection<PostDataModel> Posts { get; set; } = new List<PostDataModel>();

        public virtual ICollection<CommentDataModel> Comments { get; set; } = new List<CommentDataModel>();
    }
}
