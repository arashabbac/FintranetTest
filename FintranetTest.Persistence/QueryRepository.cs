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
    public abstract class QueryRepository<TEntity> :
        IQueryRepository<TEntity> where TEntity : class, IEntity
    {
        public QueryRepository
            (QueryDatabaseContext queryDatabaseContext)
        {
            DbSet = queryDatabaseContext.Set<TEntity>();
            DatabaseContext = queryDatabaseContext;
        }

        protected DbSet<TEntity> DbSet { get; }

        protected QueryDatabaseContext DatabaseContext { get; }

        public async Task<TEntity> FindAsync
            (int id, CancellationToken cancellationToken = default)
        {
            var result =
                await DbSet
                .FindAsync(keyValues: new object[] { id },
                cancellationToken: cancellationToken);

            return result;
        }

        public async Task<IEnumerable<TEntity>> SelectAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await DbSet.Where(predicate: predicate).ToListAsync(cancellationToken: cancellationToken);
        }
    }
}
