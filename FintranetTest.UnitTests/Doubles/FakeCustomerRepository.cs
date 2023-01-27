using FintranetTest.Domain.Aggregates;
using FintranetTest.Domain.Contracts;
using Framework.Domain;
using System.Linq.Expressions;

namespace FintranetTest.UnitTests.Doubles;

public class FakeCustomerRepository : ICustomerRepository
{

    private readonly List<Customer> _customers;

    public FakeCustomerRepository()
    {
        _customers = new List<Customer>();
    }

    public Task<Customer> AddAsync(Customer entity, CancellationToken cancellationToken = default)
    {
        _customers.Add(entity);
        return Task.FromResult(entity);
    }

    public Task AddRangeAsync(IEnumerable<Customer> entities, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<Customer> FindAsync(int id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public bool IsCustomerExist(Specification<Customer> specification)
    {
        return _customers.Any(specification.ToExpression().Compile());
    }

    public bool IsEmailAlreadyUsed(Specification<Customer> specification)
    {
        return _customers.Any(specification.ToExpression().Compile());
    }

    public Task<IEnumerable<Customer>> SelectAsync(Expression<Func<Customer, bool>> predicate, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public void Update(Customer entity)
    {
        throw new NotImplementedException();
    }

    public Task<int> SaveAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public void Delete(Customer entity)
    {
        throw new NotImplementedException();
    }
}
