using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Intecgra.Cerberus.Domain.Ports.Data;
using Microsoft.EntityFrameworkCore;

namespace Intecgra.Cerberus.Infrastructure.Data
{
    public sealed class GenericRepository<TE> : IDisposable, IGenericRepository<TE> where TE : class, new()
    {
        private readonly PersistenceContext _context;

        public GenericRepository(PersistenceContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TE>> Get(Expression<Func<TE, bool>> filter = null,
            Func<IQueryable<TE>, IOrderedQueryable<TE>> orderBy = null,
            string includeProperties = "", bool isTracking = false)
        {
            IQueryable<TE> query = _context.Set<TE>();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new[] {','}, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                return await orderBy(query).ToListAsync();
            }

            return (!isTracking)
                ? await query.AsNoTracking().ToListAsync()
                : await query.ToListAsync();
        }

        public async Task<TE> GetById(object id)
        {
            return await _context.Set<TE>().FindAsync(id);
        }


        public async Task<TE> Save(TE entity)
        {
            if (entity != null)
            {
                var item = _context.Set<TE>();
                item.Add(entity);
                await CommitAsync();
            }

            return entity;
        }

        public async Task Update(TE entity)
        {
            if (entity != null)
            {
                var item = _context.Set<TE>();
                item.Update(entity);
                await CommitAsync();
            }
        }

        public async Task Delete(TE entity)
        {
            if (entity != null)
            {
                _context.Set<TE>().Remove(entity);
                await CommitAsync();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        public async Task CommitAsync()
        {
            _context.ChangeTracker.DetectChanges();

            foreach (var entry in _context.ChangeTracker.Entries())
            {
                if (entry.State == EntityState.Added)
                {
                    //entry.Property("CreatedOn").CurrentValue = DateTime.UtcNow;
                }

                if (entry.State == EntityState.Modified)
                {
                    //entry.Property("UpdatedOn").CurrentValue = DateTime.UtcNow;
                }
            }

            await _context.SaveChangesAsync();
        }

        public async Task SaveRange(IEnumerable<TE> entity)
        {
            if (entity != null)
            {
                var item = _context.Set<TE>();

                item.AddRange(entity);
                _context.ChangeTracker.DetectChanges();

                foreach (var entry in _context.ChangeTracker.Entries())
                {
                    if (entry.State == EntityState.Added)
                    {
                        //entry.Property("CreatedOn").CurrentValue = DateTime.UtcNow;
                    }

                    if (entry.State == EntityState.Modified)
                    {
                        //entry.Property("UpdatedOn").CurrentValue = DateTime.UtcNow;
                    }
                }

                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteRange(IEnumerable<TE> entity)
        {
            if (entity != null)
            {
                var item = _context.Set<TE>();

                item.RemoveRange(entity);
                _context.ChangeTracker.DetectChanges();

                foreach (var entry in _context.ChangeTracker.Entries())
                {
                    if (entry.State == EntityState.Added)
                    {
                        //entry.Property("CreatedOn").CurrentValue = DateTime.UtcNow;
                    }

                    if (entry.State == EntityState.Modified)
                    {
                        //entry.Property("UpdatedOn").CurrentValue = DateTime.UtcNow;
                    }
                }

                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateRange(IEnumerable<TE> entity)
        {
            if (entity != null)
            {
                var item = _context.Set<TE>();
                item.UpdateRange(entity);
                await CommitAsync();
            }
        }
    }
}