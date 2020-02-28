using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Reddnet.Core.Entities
{
    public class PostEntity : EntityBase
    {
        private AuthorEntity _AuthorEntity;
        private BlogEntity _blog;
        private ICollection<ReplyEntity> _comments;

        public PostEntity() { }

        private ILazyLoader LazyLoader { get; set; }

        public PostEntity(ILazyLoader lazyLoader)
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
        public BlogEntity Blog
        {
            get => this.LazyLoader.Load(this, ref _blog);
            set => _blog = value;
        }

        public Guid? AuthorId { get; set; }
        public AuthorEntity AuthorEntity
        {
            get => this.LazyLoader.Load(this, ref _AuthorEntity);
            set => _AuthorEntity = value;
        }

        public ICollection<ReplyEntity> Comments
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
