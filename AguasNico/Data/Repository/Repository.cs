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

        public async Task AddAsync(T entity)
        {
            await dbSet.AddAsync(entity);
        }

        public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            string? includeProperties = null)
        {
            IQueryable<T> query = dbSet.AsQueryable();
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
                var list = await orderBy(query).ToListAsync();
                return list;
            }
            else
            {
                var list = await query.ToListAsync();
                return list;
            }
        }

        public async Task<T> GetFirstOrDefaultAsync(Expression<Func<T, bool>>? filter = null, string? includeProperties = null)
        {
            IQueryable<T> query = dbSet.AsQueryable();
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
            return await query.FirstOrDefaultAsync();
        }

        public async Task<T> GetOneAsync(int id)
        {
            return await dbSet.FindAsync(id) ?? throw new NullReferenceException("No se ha encontrado la entidad");
        }

        public async Task<T> GetOneAsync(short id)
        {
            return await dbSet.FindAsync(id) ?? throw new NullReferenceException("No se ha encontrado la entidad");
        }

        public async Task<T> GetOneAsync(long id)
        {
            return await dbSet.FindAsync(id) ?? throw new NullReferenceException("No se ha encontrado la entidad");
        }
    }
}