using FintranetTest.Domain.Aggregates;
using FintranetTest.Domain.Contracts;
using Framework.Domain;
using System.Linq;

namespace FintranetTest.Persistence.Repositories;

public class CustomerRepository : Repository<Customer>, ICustomerRepository
{
    public CustomerRepository(DatabaseContext databaseContext) : base(databaseContext)
    {
    }

    public bool IsCustomerExist(Specification<Customer> specification)
    {
        return DbSet.Any(specification.ToExpression().Compile());
    }

    public bool IsEmailAlreadyUsed(Specification<Customer> specification)
    {
        return DbSet.Any(specification.ToExpression().Compile());
    }
}
