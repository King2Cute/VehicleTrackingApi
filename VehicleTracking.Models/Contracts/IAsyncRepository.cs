using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace VehicleTracking.Models.Contracts
{
    public interface IAsyncRepository<T> : IDisposable where T : IEntity
    {
        Task<T> AddAsync(T entity);

        Task DeleteAsync(Guid id);

        Task DeleteAsync(T entity);

        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> filter);

        Task<IEnumerable<T>> GetAllAsync();

        Task<T> GetByIdAsync(Guid? id);

        Task UpdateAsync(T entity);
    }
}
