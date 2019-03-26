using Reddnet.Data.AccountData;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Reddnet.Models.DataModels
{
    public class BlogDataModel
    {
        [Key]
        public int BlogId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        public byte[] Cover { get; set; }

        public DateTime Created { get; set; }

        //

        public ICollection<PostDataModel> Posts { get; set; } = new List<PostDataModel>();
    }
}
