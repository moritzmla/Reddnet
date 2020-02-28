using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Reddnet.Core.Entities;
using System;

namespace Reddnet.DataAccess.Identity
{
    public class ApplicationUser : IdentityUser
    {
        private AuthorEntity _authorEntity;

        public ApplicationUser() { }

        public ILazyLoader LazyLoader { get; set; }

        public ApplicationUser(ILazyLoader lazyLoader)
        {
            this.LazyLoader = lazyLoader;
        }

        public Guid? AuthorId { get; set; }
        public AuthorEntity AuthorEntity
        {
            get => this.LazyLoader.Load(this, ref _authorEntity);
            set => _authorEntity = value;
        }
    }
}
