using Reddnet.Data.AccountData;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Reddnet.Models.DataModels
{
    public class CommentDataModel
    {
        [Key]
        public int CommentId { get; set; }

        [Required]
        public string Content { get; set; }

        public DateTime UploadDate { get; set; }

        public string AuthorId { get; set; }
        public virtual ApplicationUser Author { get; set; }

        public int PostId { get; set; }
        public virtual PostDataModel Post { get; set; }

        // Methods

        public string RenderContent()
        {
            string renderedContent = this.Content;

            renderedContent = renderedContent.Replace(Environment.NewLine, "<br />");

            return renderedContent;
        }
    }
}
