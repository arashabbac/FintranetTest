using FintranetTest.Domain.Aggregates;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace FintranetTest.Persistence
{
    public class QueryDatabaseContext : DbContext
    {
        public QueryDatabaseContext
            (DbContextOptions<QueryDatabaseContext> options) : base(options)
        {
        }
        public DbSet<Customer> Customers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

    }
}
