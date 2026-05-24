using OrderFinanceControl.UseCases.Customers;
using OrderFinanceControl.UseCases.Orders;
using OrderFinanceControl.UseCases.Products;

namespace OrderFinanceControl.Extensions;

public static class UseCaseExtensions
{
    public static IServiceCollection AddUseCases(this IServiceCollection services)
    {
        services.AddScoped<CreateCustomerUseCase>();
        services.AddScoped<CreateProductUseCase>();
        services.AddScoped<CreateOrderUseCase>();
        services.AddScoped<MarkOrderAsPaidUseCase>();
        services.AddScoped<GetCustomersUseCase>();
        services.AddScoped<GetOrdersUseCase>();
        services.AddScoped<GetOrderByIdUseCase>();
        services.AddScoped<GetProductsUseCase>();
        return services;
    }
}
