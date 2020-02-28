using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.ComponentModel.DataAnnotations;

namespace Reddnet.Core.Entities
{
    public class ReplyEntity : EntityBase
    {
        private AuthorEntity _AuthorEntity;
        private PostEntity _post;

        public ReplyEntity() { }

        private ILazyLoader LazyLoader { get; set; }

        public ReplyEntity(ILazyLoader lazyLoader)
        {
            this.LazyLoader = lazyLoader;
        }

        [Required]
        public string Content { get; set; }

        public Guid? AuthorId { get; set; }
        public AuthorEntity AuthorEntity
        {
            get => this.LazyLoader.Load(this, ref _AuthorEntity);
            set => _AuthorEntity = value;
        }

        public Guid? PostId { get; set; }
        public PostEntity Post
        {
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
