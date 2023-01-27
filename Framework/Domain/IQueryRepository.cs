using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Framework.Domain
{
    public interface IQueryRepository<T> where T : IEntity
    {
        Task<IEnumerable<T>> SelectAsync
            (Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);

        Task<T> FindAsync(int id,CancellationToken cancellationToken = default);
    }
}
