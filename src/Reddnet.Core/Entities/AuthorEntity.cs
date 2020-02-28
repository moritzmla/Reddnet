using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Reddnet.Core.Entities
{
    public class AuthorEntity : EntityBase
    {
        private ICollection<PostEntity> _posts;
        private ICollection<ReplyEntity> _comments;

        public AuthorEntity() { }

        private ILazyLoader LazyLoader { get; set; }

        public AuthorEntity(ILazyLoader lazyLoader)
        {
            this.LazyLoader = lazyLoader;
        }

        [Required]
        public string Name { get; set; }

        [Required]
        public byte[] Image { get; set; }

        public ICollection<PostEntity> Posts
        {
            get => this.LazyLoader.Load(this, ref _posts);
            set => _posts = value;
        }

        public ICollection<ReplyEntity> Comments
        {
            get => this.LazyLoader.Load(this, ref _comments);
            set => _comments = value;
        }
    }
}
