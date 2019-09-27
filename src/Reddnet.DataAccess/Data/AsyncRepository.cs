using BlogCoreEngine.Core.Entities;
using BlogCoreEngine.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlogCoreEngine.DataAccess.Data
{
    public class AsyncRepository<T> : IAsyncRepository<T> where T : BaseEntity
    {
        private readonly ApplicationDbContext context;
        private readonly ILogger<AsyncRepository<T>> logger;

        public AsyncRepository(ApplicationDbContext context, ILogger<AsyncRepository<T>> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        public async Task<T> Add(T entity)
        {
            this.logger.LogInformation("add entity", entity);
            await this.context.Set<T>().AddAsync(entity);
            await this.context.SaveChangesAsync();
            return entity;
        }

        public async Task<IReadOnlyList<T>> GetAll()
        {
            this.logger.LogInformation("get all entities");
            return await this.context.Set<T>().ToListAsync();
        }

        public async Task<T> GetById(Guid entityId)
        {
            this.logger.LogInformation("get entity with id", entityId);
            return await this.context.Set<T>().FirstOrDefaultAsync(x => x.Id == entityId);
        }

        public async Task Remove(Guid entityId)
        {
            this.logger.LogInformation("remove entity", entityId);
            this.context.Set<T>().Remove(await GetById(entityId));
            await this.context.SaveChangesAsync();
        }

        public async Task RemoveRange(IEnumerable<T> entities)
        {
            this.logger.LogInformation("remove range of entities", entities);
            this.context.Set<T>().RemoveRange(entities);
            await this.context.SaveChangesAsync();
        }

        public async Task Update(T entity)
        {
            this.logger.LogInformation("update entity", entity);
            this.context.Set<T>().Update(entity);
            await this.context.SaveChangesAsync();
        }
    }
}
