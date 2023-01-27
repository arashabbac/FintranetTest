using FintranetTest.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace FintranetTest.AcceptanceTests;

public class TestingWebAppFactory<TEntryPoint> : WebApplicationFactory<Program> where TEntryPoint : Program
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices((hostContext, services) =>
        {
            var databaseContextDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<DatabaseContext>));

            if (databaseContextDescriptor != null)
                services.Remove(databaseContextDescriptor);

            var queryDatabaseContextDescriptor = services
                .SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<QueryDatabaseContext>));

            if (queryDatabaseContextDescriptor != null)
                services.Remove(queryDatabaseContextDescriptor);

            services.AddDbContext<DatabaseContext>(options =>
            {
                options.UseSqlServer(hostContext.Configuration.GetConnectionString("TestingDatabase"));
            });

            services.AddDbContext<QueryDatabaseContext>(options =>
            {
                options.UseSqlServer(hostContext.Configuration.GetConnectionString("TestingDatabase"))
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            });

            var sp = services.BuildServiceProvider();
            using var scope = sp.CreateScope();
            using var appContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
            try
            {
                appContext.Database.EnsureCreated();
                appContext.Database.ExecuteSqlRawAsync("DELETE FROM dbo.Customers");
            }
            catch (Exception ex)
            {
                //Log or sth u need
                Console.WriteLine(ex.ToString());
                throw;
            }
        });
    }
}
