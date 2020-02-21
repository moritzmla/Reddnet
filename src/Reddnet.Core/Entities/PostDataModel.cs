using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BlogCoreEngine.Core.Entities
{
    public class PostDataModel : BaseEntity
    {
        private Author _author;
        private BlogDataModel _blog;
        private ICollection<CommentDataModel> _comments;

        public PostDataModel() { }

        private ILazyLoader LazyLoader { get; set; }

        public PostDataModel(ILazyLoader lazyLoader)
        {
            this.LazyLoader = lazyLoader;
        }

        [Required]
        public byte[] Cover { get; set; }

        public string Link { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public int Views { get; set; }

        [Required]
        public bool Archieved { get; set; }

        [Required]
        public bool Pinned { get; set; }

        public Guid? BlogId { get; set; }
        public BlogDataModel Blog
        {
            get => this.LazyLoader.Load(this, ref _blog);
            set => _blog = value;
        }

        public Guid? AuthorId { get; set; }
        public Author Author
        {
            get => this.LazyLoader.Load(this, ref _author);
            set => _author = value;
        }

        public ICollection<CommentDataModel> Comments
        {
            get => this.LazyLoader.Load(this, ref _comments);
            set => _comments = value;
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
