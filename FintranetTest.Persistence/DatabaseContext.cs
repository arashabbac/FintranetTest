using FintranetTest.Domain.Aggregates;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace FintranetTest.Persistence;

public class DatabaseContext : DbContext
{
    public DatabaseContext
    (DbContextOptions<DatabaseContext> options) : base(options)
    {
    }

    public DbSet<Customer> Customers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
