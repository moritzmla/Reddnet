using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Reddnet.Application.Interfaces;
using Reddnet.Domain.Entities;
using Reddnet.Domain.Interfaces;

namespace Reddnet.DataAccess;

public class DataContext : IdentityDbContext<UserEntity, IdentityRole<Guid>, Guid>, IDataContext
{
    private readonly IUser user;

    public DbSet<CommunityEntity> Communities { get; set; }
    public DbSet<PostEntity> Posts { get; set; }
    public DbSet<ReplyEntity> Replies { get; set; }

    public DataContext(DbContextOptions<DataContext> options, IUser user) : base(options)
    {
        this.user = user;
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        if (this.user.IsAuthenticated)
        {
            foreach (var userEntry in this.ChangeTracker.Entries<IUserEntity>())
            {
                if (userEntry.State == EntityState.Added)
                {
                    userEntry.Entity.UserId = this.user.Id;
                }
            }
        }

        foreach (var auditEntry in this.ChangeTracker.Entries<IAuditEntity>())
        {
            if (auditEntry.State == EntityState.Modified)
            {
                auditEntry.Entity.Modified = DateTimeOffset.UtcNow;
            }

            if (auditEntry.State == EntityState.Added)
            {
                auditEntry.Entity.Created = DateTimeOffset.UtcNow;
                auditEntry.Entity.Modified = DateTimeOffset.UtcNow;
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
