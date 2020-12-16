using Microsoft.EntityFrameworkCore;
using Reddnet.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace Reddnet.Application.Interfaces
{
    public interface IDataContext
    {
        DbSet<CommunityEntity> Communities { get; set; }
        DbSet<PostEntity> Posts { get; set; }
        DbSet<ReplyEntity> Replies { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
