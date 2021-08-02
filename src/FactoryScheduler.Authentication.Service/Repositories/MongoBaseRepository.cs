using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using FactoryScheduler.Authentication.Service.Entities;
using FactoryScheduler.Authentication.Service.Interfaces;
using MongoDB.Driver;

namespace FactoryScheduler.Authentication.Service.Repositories
{

    public class MongoBaseRepository<T> : IMongoBaseRepository<T> where T : IMongoEntity
    {

        private readonly IMongoCollection<T> _dbCollection;

        private readonly FilterDefinitionBuilder<T> filterBuilder = Builders<T>.Filter;

        public MongoBaseRepository(IMongoCollection<T> collection)
        {
            _dbCollection = collection;
        }

        public async Task<IReadOnlyCollection<T>> GetAllAsync()
        {
            return await _dbCollection.Find(filterBuilder.Empty).ToListAsync();
        }

        public async Task<IReadOnlyCollection<T>> GetAllAsync(Expression<Func<T, bool>> filter)
        {
            return await _dbCollection.Find(filter).ToListAsync();
        }

        public async Task<T> GetOneAsync(Guid id)
        {
            FilterDefinition<T> filter = filterBuilder.Eq(p => p.Id, id);
            return await _dbCollection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<T> GetFirstOrDefaultAsync(Expression<Func<T, bool>> filter)
        {
            return await _dbCollection.Find(filter).FirstOrDefaultAsync();
        }


        public async Task CreateAsync(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            await _dbCollection.InsertOneAsync(entity);
        }

        public async Task UpdateAsync(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            FilterDefinition<T> filter = filterBuilder.Eq(p => p.Id, entity.Id);
            await _dbCollection.ReplaceOneAsync(filter, entity);
        }

        public async Task RemoveAsync(Guid id)
        {
            FilterDefinition<T> filter = filterBuilder.Eq(p => p.Id, id);
            await _dbCollection.DeleteOneAsync(filter);
        }
    }
}