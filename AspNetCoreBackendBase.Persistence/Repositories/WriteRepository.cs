using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using AspNetCoreBackendBase.Application.Repositories;
using AspNetCoreBackendBase.Domain.Entities;
using AspNetCoreBackendBase.Persistence.Context;

namespace AspNetCoreBackendBase.Persistence.Repositories
{
    public class WriteRepository<T, TKey> : IWriteRepository<T, TKey> where T : BaseEntity<TKey>
    {
        readonly AspNetCoreBackendBaseDbContext _context;
        public DbSet<T> Table => _context.Set<T>();

        public WriteRepository(AspNetCoreBackendBaseDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AddAsync(T model)
        {
            EntityEntry<T> entityEntry = await Table.AddAsync(model);
            return entityEntry.State == EntityState.Added;
        }

        public async Task<bool> AddRangeAsync(List<T> models)
        {
            await Table.AddRangeAsync(models);
            return true;
        }

        public bool Remove(T model)
        {
            EntityEntry<T> entityEntry = Table.Remove(model);
            return entityEntry.State == EntityState.Deleted;
        }

        public bool RemoveRange(List<T> models)
        {
            Table.RemoveRange(models);
            return true;
        }

        public async Task<bool> RemoveAsync(TKey id)
        {
            // Use EqualityComparer for comparing TKey
            T? model = await Table.FirstOrDefaultAsync(data => EqualityComparer<TKey>.Default.Equals(data.Id, id));
            if (model == null)
                return false;
            return Remove(model);
        }

        public bool Update(T model)
        {
            EntityEntry entityEntry = Table.Update(model);
            return entityEntry.State == EntityState.Modified;
        }

        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
