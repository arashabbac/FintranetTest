using Framework.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace FintranetTest.Persistence
{
    public abstract class Repository<T> :
        IRepository<T> where T : class, IAggregateRoot
    {
        public Repository
            (DatabaseContext databaseContext)
        {
            DbSet = databaseContext.Set<T>();
            DatabaseContext = databaseContext;
        }

        protected DbSet<T> DbSet { get; }

        protected DatabaseContext DatabaseContext { get; }

        public async Task<T> AddAsync
        (T entity, CancellationToken cancellationToken = default)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(paramName: nameof(entity));
            }

            var result =
                await DbSet.AddAsync
                (entity: entity, cancellationToken: cancellationToken);

            return result.Entity;
        }

        public async Task AddRangeAsync
            (IEnumerable<T> entities, CancellationToken cancellationToken = default)
        {
            if (entities == null)
            {
                throw new ArgumentNullException(paramName: nameof(entities));
            }

            await DbSet.AddRangeAsync
                (entities: entities, cancellationToken: cancellationToken);
        }

        public void Update(T entity)
        {
            DbSet.Update(entity: entity);
        }

        public async Task<T> FindAsync
            (int id, CancellationToken cancellationToken = default)
        {
            var result =
                await DbSet
                .FindAsync(keyValues: new object[] { id },
                cancellationToken: cancellationToken);

            return result;
        }

        public async Task<IEnumerable<T>> SelectAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await DbSet.Where(predicate).ToListAsync(cancellationToken: cancellationToken);
        }

        public async Task<int> SaveAsync(CancellationToken cancellationToken = default)
        {
            var result =
                await DatabaseContext.SaveChangesAsync(cancellationToken);

            return result;
        }

        public void Delete(T entity)
        {
            DbSet.Remove(entity: entity);
        }
    }
}
