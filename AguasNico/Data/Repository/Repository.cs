using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AguasNico.Data.Repository.IRepository;

namespace AguasNico.Data.Repository
{
    public class Repository<T>(DbContext context) : IRepository<T> where T : class
    {
        protected readonly DbContext _context = context;
        internal DbSet<T> dbSet = context.Set<T>();

        public void Add(T entity)
        {
            dbSet.Add(entity);
        }

        public IEnumerable<T> GetAll(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includeProperties = null)
        {
            IQueryable<T> query = dbSet;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            // Include properties separados por coma
            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty.Trim());
                }
            }

            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }
            return query.ToList();
        }

        public T GetFirstOrDefault(Expression<Func<T, bool>> filter = null, string includeProperties = null)
        {
            IQueryable<T> query = dbSet;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            // Include properties separados por coma
            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty.Trim());
                }
            }
            return query.FirstOrDefault();
        }

        public T GetOne(int id) => dbSet.Find(id);
        public T GetOne(short id) => dbSet.Find(id);
        public T GetOne(long id) => dbSet.Find(id);

        public void Remove(int id)
        {
            T entityToRemove = dbSet.Find(id);
            this.Remove(entityToRemove);
        }
        public void Remove(short id)
        {
            T entityToRemove = dbSet.Find(id);
            this.Remove(entityToRemove);
        }
        public void Remove(long id)
        {
            T entityToRemove = dbSet.Find(id);
            this.Remove(entityToRemove);
        }

        public void Remove(T entity)
        {
            dbSet.Remove(entity);
        }
    }
}