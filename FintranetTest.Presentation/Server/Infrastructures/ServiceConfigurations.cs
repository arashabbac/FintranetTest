using FintranetTest.Domain.Contracts;
using FintranetTest.Persistence;
using FintranetTest.Persistence.QueryRepositories;
using FintranetTest.Persistence.Repositories;
using FintranetTest.Presentation.Server.Commands;
using FintranetTest.Presentation.Server.Validators;
using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FintranetTest.Presentation.Server.Infrastructures;

public static class ServiceConfigurations
{
    public static void AddRequiredServices(this IServiceCollection services)
    {
        services.AddControllers(options =>
        {
            options.Filters.Add(typeof(ValidateModelStateAttribute));
        });
        services.AddFluentValidationAutoValidation(o => o.DisableDataAnnotationsValidation = true);
        services.AddValidatorsFromAssemblyContaining<CreateCustomerCommandValidator>();

        services.AddMediatR(typeof(CreateCustomerCommand).Assembly);
        services.Configure<ApiBehaviorOptions>(options => { options.SuppressModelStateInvalidFilter = true; });
        services.AddSwaggerGen();
    }

    public static void AddDatabases(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<DatabaseContext>(options =>
        {
            var connectionString = configuration.GetConnectionString("ConnectionString");
            options.UseSqlServer(connectionString);
        }, ServiceLifetime.Scoped);

        services.AddDbContext<QueryDatabaseContext>(options =>
        {
            var connectionString = configuration.GetConnectionString("ConnectionString");
            options.UseSqlServer(connectionString).UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        });
    }

    public static void AddRepositories(this IServiceCollection services)
    {
        services.AddTransient<ICustomerRepository, CustomerRepository>();
        services.AddTransient<ICustomerQueryRepository, CustomerQueryRepository>();
    }
}
