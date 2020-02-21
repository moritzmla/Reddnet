using BlogCoreEngine.Core.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlogCoreEngine.Core.Interfaces
{
    public interface IAsyncRepository<T> where T : BaseEntity
    {
        Task<T> Add(T entity);

        Task Remove(Guid entityId);

        Task Update(T entity);

        Task<IReadOnlyList<T>> GetAll();

        Task<T> GetById(Guid entityId);

        Task RemoveRange(IEnumerable<T> entities);
    }
}
