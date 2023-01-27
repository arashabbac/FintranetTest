using FintranetTest.Domain.Aggregates;
using FintranetTest.Persistence.DTOs;
using Framework.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FintranetTest.Persistence.QueryRepositories;

public interface ICustomerQueryRepository : IQueryRepository<Customer>
{
    Task<IList<CustomerDto>> GetAllAsync();
    Task<CustomerDto> GetByIdAsync(int id);
}