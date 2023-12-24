using System.Linq.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AguasNico.Data.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        T GetOne(int id);
        T GetOne(long id);
        T GetOne(short id);

        IEnumerable<T> GetAll(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includeProperties = null
        );

        T GetFirstOrDefault(
            Expression<Func<T, bool>> filter = null,
            string includeProperties = null
        );

        void Add(T entity);

        void Remove(int id);
        void Remove(short id);
        void Remove(long id);

        void Remove (T entity);
    }
}