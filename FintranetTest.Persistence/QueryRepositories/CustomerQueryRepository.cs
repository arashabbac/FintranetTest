using FintranetTest.Domain.Aggregates;
using FintranetTest.Persistence.DTOs;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FintranetTest.Persistence.QueryRepositories;

public class CustomerQueryRepository : QueryRepository<Customer>, ICustomerQueryRepository
{
    public CustomerQueryRepository(QueryDatabaseContext queryDatabaseContext) : base(queryDatabaseContext)
    {
    }

    public async Task<IList<CustomerDto>> GetAllAsync()
    {
        return await DbSet.OrderByDescending(c => c.Id).Select(c => new CustomerDto
        {
            Id = c.Id,
            Firstname = c.Firstname.Value,
            Lastname = c.Lastname.Value,
            DateOfBirth = c.DateOfBirth,
            BankAccountNumber = c.BankAccountNumber.Value,
            Email = c.Email.Value,
            PhoneNumber = c.PhoneNumber.Value
        }).ToListAsync();
    }

    public async Task<CustomerDto> GetByIdAsync(int id)
    {
        var data = await DbSet.FirstOrDefaultAsync(c => c.Id == id);

        if (data is null)
            return null;

        return new CustomerDto
        {
            Id = data.Id,
            Email = data.Email.Value,
            Lastname = data.Lastname.Value,
            DateOfBirth = data.DateOfBirth,
            Firstname = data.Firstname.Value,
            PhoneNumber = data.PhoneNumber.Value,
            BankAccountNumber = data.BankAccountNumber.Value,
        };
    }
}
