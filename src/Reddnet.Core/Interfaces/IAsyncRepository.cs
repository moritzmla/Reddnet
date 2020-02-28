using Reddnet.Core.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Reddnet.Core.Interfaces
{
    public interface IAsyncRepository<T> where T : EntityBase
    {
        Task<T> Add(T entity);

        Task Remove(Guid entityId);

        Task Update(T entity);

        Task<IReadOnlyList<T>> GetAll();

        Task<IReadOnlyList<T>> Get(Func<T, bool> predicate);

        Task<T> GetById(Guid entityId);

        Task RemoveRange(IEnumerable<T> entities);
    }
}
