using Microsoft.EntityFrameworkCore;
using NoteForge.Domain.Interfaces.Repositories;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace NoteForge.Infrastructure.Repositories
{
    internal class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        protected readonly AppDbContext appDbContext;
        protected readonly DbSet<T> dbSet;

        public BaseRepository(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
            dbSet = appDbContext.Set<T>();
        }

        // Tag helpers
        protected IQueryable<T> TaggedNoTracking([CallerMemberName] string caller = "")
            => dbSet.AsNoTracking().TagWith($"{GetType().Name}.{caller}");

        public Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
            => TaggedNoTracking()
                .FirstOrDefaultAsync(x => EF.Property<int>(x, "Id") == id);

        public async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
            => await TaggedNoTracking()
                .AnyAsync(predicate, cancellationToken);

        public async Task AddAsync(T entity, CancellationToken cancellationToken = default)
            => await dbSet.AddAsync(entity, cancellationToken);

        public async Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
            => await dbSet.AddRangeAsync(entities, cancellationToken);

        public void Update(T entity)
            => dbSet.Update(entity);

        public void Remove(T entity)
            => dbSet.Remove(entity);

        public void RemoveRange(IEnumerable<T> entities)
            => dbSet.RemoveRange(entities);
    }
}
