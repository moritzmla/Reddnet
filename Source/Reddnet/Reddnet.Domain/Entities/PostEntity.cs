using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;

namespace Reddnet.Domain.Entities
{
    public record PostEntity : EntityBase
    {
        private UserEntity _user;
        private CommunityEntity _community;
        private ICollection<ReplyEntity> _replies;
        private readonly ILazyLoader lazyLoader;

        public PostEntity() { }
        public PostEntity(ILazyLoader lazyLoader)
        {
            this.lazyLoader = lazyLoader;
        }

        public string Title { get; set; }
        public string Content { get; set; }
        public byte[] Image { get; set; }

        public Guid? UserId { get; set; }
        public UserEntity User
        {
            get => this.lazyLoader.Load(this, ref _user);
            set => _user = value;
        }

        public Guid? CommunityId { get; set; }
        public CommunityEntity Community
        {
            get => this.lazyLoader.Load(this, ref _community);
            set => _community = value;
        }

        public ICollection<ReplyEntity> Replies
        {
            get => this.lazyLoader.Load(this, ref _replies);
            set => _replies = value;
        }
    }
}
