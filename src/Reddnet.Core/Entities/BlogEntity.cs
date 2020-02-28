using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Reddnet.Core.Entities
{
    public class BlogEntity : EntityBase
    {
        private ICollection<PostEntity> _posts;

        public BlogEntity() { }

        private ILazyLoader LazyLoader { get; set; }

        public BlogEntity(ILazyLoader lazyLoader)
        {
            this.LazyLoader = lazyLoader;
        }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public byte[] Cover { get; set; }

        public ICollection<PostEntity> Posts
        {
            get => this.LazyLoader.Load(this, ref _posts);
            set => _posts = value;
        }
    }
}
