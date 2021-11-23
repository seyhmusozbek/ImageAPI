using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ImageAPI.IRepository
{
    public interface IGenericRepository<T> where T: class
    {
        Task<IList<T>> GetAll(
            Expression<Func<T, bool>> expression = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            List<string> includes = null
            );

        Task<T> Get(Expression<Func<T, bool>> expression = null, List<string> includes = null);

        Task InsertOne(T entity);
        Task InsertMany(IEnumerable<T> entities);
        Task DeleteOne(Expression<Func<T, bool>> expression = null);
        Task DeleteMany(Expression<Func<T, bool>> expression = null);
        void Update(T entity);
    }
}
