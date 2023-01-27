using FintranetTest.Domain.Aggregates;
using Framework.Domain;
using System.Numerics;

namespace FintranetTest.Domain.Contracts;

public interface ICustomerRepository : IRepository<Customer>
{
    bool IsEmailAlreadyUsed(Specification<Customer> specification);
    bool IsCustomerExist(Specification<Customer> specification);
}
