﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Framework.Domain;

public interface IRepository<T> where T : IAggregateRoot
{
    Task<T> AddAsync
        (T entity, CancellationToken cancellationToken = default);

    Task AddRangeAsync
        (IEnumerable<T> entities, CancellationToken cancellationToken = default);

    void Update(T entity);

    Task<IEnumerable<T>> SelectAsync
        (Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);

    Task<T> FindAsync
        (int id, CancellationToken cancellationToken = default);
    Task<int> SaveAsync
    (CancellationToken cancellationToken = default);

    void Delete (T entity);

}