using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FactoryScheduler.Authentication.Service.Interfaces
{
    public interface IMongoBaseRepository<T> where T : IMongoEntity
    {
        Task CreateAsync(T entity);
        Task<IReadOnlyCollection<T>> GetAllAsync();
        Task<IReadOnlyCollection<T>> GetAllAsync(Expression<Func<T, bool>> filter);
        Task<T> GetFirstOrDefaultAsync(Expression<Func<T, bool>> filter);
        Task<T> GetOneAsync(Guid id);
        Task RemoveAsync(Guid id);
        Task UpdateAsync(T entity);
    }
}