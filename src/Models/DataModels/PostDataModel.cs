using Reddnet.Data.AccountData;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Reddnet.Models.DataModels
{
    public class PostDataModel
    {
        [Key]
        public int PostId { get; set; }

        public byte[] Cover { get; set; }

        public string Link { get; set; }

        [Required]
        public string Title { get; set; }

        public string Preview { get; set; }

        [Required]
        public string Content { get; set; }

        public string Tags { get; set; }

        public int Views { get; set; }

        public DateTime UploadDate { get; set; }

        public DateTime LastChangeDate { get; set; }

        public bool Archieved { get; set; }

        public bool Pinned { get; set; }

        public int BlogId { get; set; }
        public virtual BlogDataModel Blog { get; set; }

        public string AuthorId { get; set; }
        public virtual ApplicationUser Author { get; set; }

        public virtual ICollection<CommentDataModel> Comments { get; set; } = new List<CommentDataModel>();

        // Methods

        public string RenderContent()
        {
            string renderedContent = this.Content;

            renderedContent = renderedContent.Replace(Environment.NewLine, "<br />");

            return renderedContent;
        }

        public string RenderPreview()
        {
            string renderedPreview = this.Preview;

            renderedPreview = renderedPreview.Replace(Environment.NewLine, "<br />");

            return renderedPreview;
        }
    }
}
