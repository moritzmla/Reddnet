using Microsoft.EntityFrameworkCore;
using Reddnet.Domain.Entities;

namespace Reddnet.Application.Interfaces;

public interface IDataContext
{
    DbSet<CommunityEntity> Communities { get; set; }
    DbSet<PostEntity> Posts { get; set; }
    DbSet<ReplyEntity> Replies { get; set; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
