using OrderFinanceControl.Data.Repositories.Interfaces;
using OrderFinanceControl.Data.Repositories.Sql;

namespace OrderFinanceControl.Extensions;

public static class RepositoryExtensions
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IProductRepository, ProductSqlRepository>();
        services.AddScoped<IOrderRepository, OrderSqlRepository>();
        services.AddScoped<ICustomerRepository, CustomerSqlRepository>();
        return services;

    }
}
