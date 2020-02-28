using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Reddnet.Core.Entities;
using Reddnet.DataAccess.Identity;

namespace Reddnet.DataAccess
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<AuthorEntity> Authors { get; set; }
        public DbSet<PostEntity> Posts { get; set; }
        public DbSet<ReplyEntity> Comments { get; set; }
        public DbSet<BlogEntity> Blogs { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            ApplicationSeed.ApplySeed(builder);
            builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        }
    }
}
