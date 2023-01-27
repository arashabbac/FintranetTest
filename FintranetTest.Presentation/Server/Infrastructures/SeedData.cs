using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using FintranetTest.Persistence;
using FintranetTest.Domain.Contracts;
using FintranetTest.Domain.Aggregates;
using FintranetTest.Domain.ValueObjects;

namespace FintranetTest.Presentation.Server.Infrastructures;

public static class SeedData
{
    public static void AutoMigrationAndSeedData(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.GetService<IServiceScopeFactory>()!.CreateScope();
        using var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
        context.Database.EnsureCreated();

        var executedSeedings = context.Customers.Any();
        if (executedSeedings == false)
        {
            var customerRepository = scope.ServiceProvider.GetRequiredService<ICustomerRepository>();
            using var transaction = context.Database.BeginTransaction();
            try
            {
                for (int index = 0; index < 150; index++)
                {
                    var customer = Customer.Create(Name.Create($"Firstname {index}").Value,
                        Name.Create($"Lastname {index}").Value,
                        DateOnly.FromDateTime(DateTime.Now.AddYears(-40).AddDays(index)),
                        PhoneNumber.Create($"09121112211").Value,
                        Email.Create($"something{index}@gmail.com").Value,
                        BankAccountNumber.Create($"32424234242{index}").Value,
                        customerRepository).Value;

                    customerRepository.AddAsync(customer);
                }

                context.SaveChanges();
                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
            }

        }

    }
}
