using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BlogCoreEngine.Web.ViewModels
{
    public class PostViewModel
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string Text { get; set; }

        [DataType(DataType.Url)]
        public string Link { get; set; }

        public byte[] Cover { get; set; }
    }
}
