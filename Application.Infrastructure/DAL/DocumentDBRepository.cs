using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Application.Core.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Application.Infrastructure.DAL
{
    public class DocumentDbRepository<T> where T : class
    {
        protected IMongoDatabase database;

        private MongoClient client;

        protected IMongoCollection<T> _collection;

        public void Initialize()
        {
            client = new MongoClient(AppSettingsProvider.MongoDbConnectionString);
            database = client.GetDatabase(AppSettingsProvider.MongoDbDatabase);
        }

        public async Task<List<T>> Get(Expression<Func<T, bool>> predicate)
        {
            return await _collection.FindAsync(predicate).Result.ToListAsync();
        }

        public async Task<T> Create(T item)
        {
            await _collection.InsertOneAsync(item);
            return item;
        }

        public async Task Update(FilterDefinition<T> filter, T item)
        {
            await _collection.ReplaceOneAsync(filter, item);
        }

        public async Task Remove(ObjectId id, Expression<Func<T, bool>> predicate)
        {
            await _collection.DeleteOneAsync(predicate);
        }
    }
}