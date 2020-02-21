using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BlogCoreEngine.Core.Entities
{
    public class BlogDataModel : BaseEntity
    {
        private ICollection<PostDataModel> _posts;

        public BlogDataModel() { }

        private ILazyLoader LazyLoader { get; set; }

        public BlogDataModel(ILazyLoader lazyLoader)
        {
            this.LazyLoader = lazyLoader;
        }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public byte[] Cover { get; set; }

        public ICollection<PostDataModel> Posts
        {
            get => this.LazyLoader.Load(this, ref _posts);
            set => _posts = value;
        }
    }
}
