using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OrderFinanceControl.Data;
using OrderFinanceControl.Dtos.Customers;
using OrderFinanceControl.Dtos.Orders;
using OrderFinanceControl.Dtos.Products;
using OrderFinanceControl.Entities;

namespace OrderFinanceControl.Tests.Integration;

[CollectionDefinition(nameof(SqlServerIntegrationCollection))]
public sealed class SqlServerIntegrationCollection : ICollectionFixture<SqlServerIntegrationFixture>;

[Collection(nameof(SqlServerIntegrationCollection))]
public class SqlServerOrderApiIntegrationTests(SqlServerIntegrationFixture fixture)
{
    private readonly SqlServerIntegrationFixture _fixture = fixture;

    [Fact]
    public async Task CreateCustomerAndListCustomers_ShouldPersistInSqlServer()
    {
        var client = _fixture.Factory.CreateClient();
        var email = $"joao.{Guid.NewGuid():N}@example.com";

        var createResponse = await client.PostAsJsonAsync("/Customers", new CustomerDto
        {
            Name = "Joao",
            Email = email
        });

        createResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var customersResponse = await client.GetFromJsonAsync<List<Customer>>("/Customers");
        customersResponse.Should().NotBeNull();
        customersResponse!.Should().ContainSingle(c => c.Email == email);

        using var scope = _fixture.Factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<OrderFinanceControlDbContext>();
        var persistedCustomer = await dbContext.Customers.FirstOrDefaultAsync(c => c.Email == email);

        persistedCustomer.Should().NotBeNull();
        persistedCustomer!.Name.Should().Be("Joao");
    }

    [Fact]
    public async Task CreateProductAndCreateOrder_ShouldPersistOrderAndReturnIt()
    {
        var client = _fixture.Factory.CreateClient();

        var customerEmail = $"maria.{Guid.NewGuid():N}@example.com";
        var customerResponse = await client.PostAsJsonAsync("/Customers", new CustomerDto
        {
            Name = "Maria",
            Email = customerEmail
        });
        customerResponse.EnsureSuccessStatusCode();

        var productName = $"Mouse-{Guid.NewGuid():N}";
        var productResponse = await client.PostAsJsonAsync("/Product", new ProductDto
        {
            Name = productName,
            Price = 50.0m
        });
        productResponse.StatusCode.Should().Be(HttpStatusCode.Created);

        using var scope = _fixture.Factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<OrderFinanceControlDbContext>();
        var customer = await dbContext.Customers.FirstAsync(c => c.Email == customerEmail);
        var product = await dbContext.Products.FirstAsync(p => p.Name == productName);

        var orderCreateResponse = await client.PostAsJsonAsync("/Orders", new OrderDto
        {
            CustomerId = customer.Id,
            Items = [new OrderItemDto { ProductId = product.Id, Quantity = 2 }]
        });
        orderCreateResponse.StatusCode.Should().Be(HttpStatusCode.Created);

        var orderCreated = await orderCreateResponse.Content.ReadFromJsonAsync<Dictionary<string, int>>();
        orderCreated.Should().NotBeNull();
        orderCreated!.Should().ContainKey("id");

        var orderId = orderCreated["id"];
        var orderResponse = await client.GetAsync($"/Orders/{orderId}");
        orderResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var order = await orderResponse.Content.ReadFromJsonAsync<OrderResponseDto>();
        order.Should().NotBeNull();
        order!.Customer.Name.Should().Be("Maria");
        order.Items.Should().ContainSingle();
        order.Items[0].ProductName.Should().Be(productName);
        order.TotalAmount.Should().Be(100.0m);

        var persistedOrder = await dbContext.Orders
            .Include(o => o.Items)
            .Include(o => o.Customer)
            .FirstOrDefaultAsync(o => o.Id == orderId);

        persistedOrder.Should().NotBeNull();
        persistedOrder!.Items.Should().ContainSingle();
        persistedOrder.TotalAmount.Should().Be(100.0m);
    }
}
