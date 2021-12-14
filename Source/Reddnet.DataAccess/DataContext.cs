using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Reddnet.Application.Interfaces;
using Reddnet.Domain.Entities;

namespace Reddnet.DataAccess;

public class DataContext : IdentityDbContext<UserEntity, IdentityRole<Guid>, Guid>, IDataContext
{
    public DbSet<CommunityEntity> Communities { get; set; }
    public DbSet<PostEntity> Posts { get; set; }
    public DbSet<ReplyEntity> Replies { get; set; }

    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {

    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        foreach (var entry in this.ChangeTracker.Entries<Entity>())
        {
            switch (entry.State)
            {
                case EntityState.Modified:
                    entry.Entity.Modified = DateTimeOffset.UtcNow;
                    break;
                case EntityState.Added:
                    entry.Entity.Created = DateTimeOffset.UtcNow;
                    break;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(typeof(DataContext).Assembly);
    }
}
