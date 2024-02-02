using System.Linq.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AguasNico.Data.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        Task<T> GetOneAsync(int id);
        Task<T> GetOneAsync(long id);
        Task<T> GetOneAsync(short id);

        Task<List<T>> GetAllAsync(
            Expression<Func<T, bool>>? filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            string? includeProperties = null
        );

        Task<T> GetFirstOrDefaultAsync(
            Expression<Func<T, bool>>? filter = null,
            string? includeProperties = null
        );

        Task AddAsync(T entity);
    }
}