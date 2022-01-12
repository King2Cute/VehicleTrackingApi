using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using VehicleTracking.Models.Contracts;

namespace VehicleTracking.Core.Persistence.Repositories
{
    public class BaseRepository<T> : IAsyncRepository<T> where T : IEntity
    {
        public BaseRepository(IMongoContext context, string name = null)
        {
            Context = context;

            DbSet = Context.GetCollection<T>(name ?? $"{typeof(T).Name}s");
        }

        public virtual async Task<T> AddAsync(T entity)
        {
            if (entity.Id == null || entity.Id == Guid.Empty)
                entity.Id = Guid.NewGuid();

            Context.AddCommand(() => DbSet.InsertOneAsync(entity));

            return await Task.FromResult(entity);
        }

        public virtual async Task DeleteAsync(Guid id)
        {
            Context.AddCommand(() => DbSet.DeleteOneAsync(Builders<T>.Filter.Eq("_id", id)));

            await Task.CompletedTask;
        }

        public virtual async Task DeleteAsync(T entity)
        {
            Context.AddCommand(() => DbSet.DeleteOneAsync(e => e.Id == entity.Id));

            await Task.CompletedTask;
        }

        public void Dispose()
        {
            Context?.Dispose();
            GC.SuppressFinalize(this);
        }

        public virtual async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> filter)
        {
            var result = await DbSet.FindAsync(filter);

            return result.ToList();
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            var result = await DbSet.FindAsync(Builders<T>.Filter.Empty);

            return result.ToList().Where(x => x.Id != null && x.Id != Guid.Empty);
        }

        public virtual async Task<T> GetByIdAsync(Guid? id)
        {
            var result = await DbSet.FindAsync(e => e.Id == id);

            return result.SingleOrDefault();
        }

        public virtual async Task UpdateAsync(T entity)
        {
            //await DbSet.ReplaceOneAsync(e => e.Id == entity.Id, entity);

            Context.AddCommand(() => DbSet.ReplaceOneAsync(e => e.Id == entity.Id, entity));

            await Task.CompletedTask;
        }

        protected readonly IMongoContext Context;
        protected IMongoCollection<T> DbSet;
    }
}
