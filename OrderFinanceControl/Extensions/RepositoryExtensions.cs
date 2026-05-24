using OrderFinanceControl.Data.Repositories.Interfaces;
using OrderFinanceControl.Data.Repositories.Mongo;

namespace OrderFinanceControl.Extensions;

public static class RepositoryExtensions
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IProductRepository, ProductMongoRepository>();
        services.AddScoped<IOrderRepository, OrderMongoRepository>();
        services.AddScoped<ICustomerRepository, CustomerMongoRepository>();
        return services;
    }
}
