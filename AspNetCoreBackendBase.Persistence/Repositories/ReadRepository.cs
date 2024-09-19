using AspNetCoreBackendBase.Application.Repositories;
using AspNetCoreBackendBase.Domain.Entities;
using AspNetCoreBackendBase.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace AspNetCoreBackendBase.Persistence.Repositories
{
    public class ReadRepository<T, TKey> : IReadRepository<T, TKey> where T : BaseEntity<TKey>
    {
        readonly AspNetCoreBackendBaseDbContext _context;
        public DbSet<T> Table => _context.Set<T>();

        public ReadRepository(AspNetCoreBackendBaseDbContext context)
        {
            _context = context;
        }

        public IQueryable<T> GetAll(bool tracking = true)
        {
            var query = Table.AsQueryable();
            if (!tracking)
                query = query.AsNoTracking();
            return query;
        }

        public IQueryable<T> GetWhere(Expression<Func<T, bool>> method, bool tracking = true)
        {
            var query = Table.Where(method);
            if (!tracking)
                query = query.AsNoTracking();
            return query;
        }

        public async Task<T?> GetSingleAsync(Expression<Func<T, bool>> method, bool tracking = true)
        {
            var query = Table.AsQueryable();
            if (!tracking)
                query = Table.AsNoTracking();
            return await query.FirstOrDefaultAsync(method);
        }

        public async Task<T?> GetByIdAsync(TKey id, bool tracking = true)
        {
            var query = Table.AsQueryable();
            if (!tracking)
                query = Table.AsNoTracking();
            // Use EqualityComparer for comparing TKey
            return await query.FirstOrDefaultAsync(data => EqualityComparer<TKey>.Default.Equals(data.Id, id));
        }
    }
}
