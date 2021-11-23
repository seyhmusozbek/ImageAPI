using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ImageAPI.Entities;
using ImageAPI.IRepository;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ImageAPI.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly IMongoCollection<T> _itemsCollection;


        public GenericRepository(IMongoCollection<T> itemsCollection)
        {
            _itemsCollection = itemsCollection;
        }
        public async Task DeleteOne(Expression<Func<T, bool>> expression)
        {
            await _itemsCollection.DeleteOneAsync<T>(expression);
        }

        public async Task DeleteMany(Expression<Func<T, bool>> expression)
        {
            await _itemsCollection.DeleteManyAsync<T>(expression);
        }

        public async Task<T> Get(Expression<Func<T, bool>> expression, List<string> includes = null)
        {
            return await _itemsCollection.Find<T>(expression).SingleOrDefaultAsync();
        }

        public async Task<IList<T>> GetAll(Expression<Func<T, bool>> expression = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, List<string> includes = null)
        {
            if (expression != null)
                return await _itemsCollection.Find<T>(expression).ToListAsync();
            else
                return await _itemsCollection.Find<T>(new BsonDocument()).ToListAsync();
        }

        public async Task InsertOne(T entity)
        {
            await _itemsCollection.InsertOneAsync(entity);
        }

        public async Task InsertMany(IEnumerable<T> entities)
        {
            await _itemsCollection.InsertManyAsync(entities);
        }

        public void Update(T entity)
        {
            
        }
    }
}
