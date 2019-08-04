using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BlogCoreEngine.Core.Entities
{
    public class CommentDataModel : BaseEntity
    {
        private Author _author;
        private PostDataModel _post;

        public CommentDataModel() { }

        private ILazyLoader LazyLoader { get; set; }

        public CommentDataModel(ILazyLoader lazyLoader)
        {
            this.LazyLoader = lazyLoader;
        }

        [Required]
        public string Content { get; set; }

        public Guid? AuthorId { get; set; }
        public Author Author {
            get => this.LazyLoader.Load(this, ref _author);
            set => _author = value;
        }

        public Guid? PostId { get; set; }
        public PostDataModel Post {
            get => this.LazyLoader.Load(this, ref _post);
            set => _post = value;
        }

        // Methods

        public string RenderContent()
        {
            string renderedContent = this.Content;

            renderedContent = renderedContent.Replace(Environment.NewLine, "<br />");

            return renderedContent;
        }
    }
}
