using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;

namespace Reddnet.Domain.Entities
{
    public class UserEntity : IdentityUser<Guid>
    {
        private ICollection<PostEntity> _posts;
        private ICollection<ReplyEntity> _replies;
        private readonly ILazyLoader lazyLoader;

        public UserEntity() { }
        public UserEntity(ILazyLoader lazyLoader)
        {
            this.lazyLoader = lazyLoader;
        }

        public ICollection<PostEntity> Posts
        {
            get => this.lazyLoader.Load(this, ref _posts);
            set => _posts = value;
        }

        public ICollection<ReplyEntity> Replies
        {
            get => this.lazyLoader.Load(this, ref _replies);
            set => _replies = value;
        }
    }
}
