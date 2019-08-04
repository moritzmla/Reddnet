using BlogCoreEngine.Core.Entities;
using BlogCoreEngine.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlogCoreEngine.DataAccess.Data
{
    public class AsyncRepository<T> : IAsyncRepository<T> where T : BaseEntity
    {
        private readonly ApplicationDbContext context;

        public AsyncRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<T> Add(T entity)
        {
            await this.context.Set<T>().AddAsync(entity);
            await this.context.SaveChangesAsync();
            return entity;
        }

        public async Task<IReadOnlyList<T>> GetAll()
        {
            return await this.context.Set<T>().ToListAsync();
        }

        public async Task<T> GetById(Guid entityId)
        {
            return await this.context.Set<T>().FirstOrDefaultAsync(x => x.Id == entityId);
        }

        public async Task Remove(Guid entityId)
        {
            this.context.Set<T>().Remove(await GetById(entityId));
            await this.context.SaveChangesAsync();
        }

        public async Task RemoveRange(IEnumerable<T> entities)
        {
            this.context.Set<T>().RemoveRange(entities);
            await this.context.SaveChangesAsync();
        }

        public async Task Update(T entity)
        {
            this.context.Set<T>().Update(entity);
            await this.context.SaveChangesAsync();
        }
    }
}
