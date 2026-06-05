using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using OrderFinanceControl.Data;
using OrderFinanceControl.Dtos.Customers;
using OrderFinanceControl.Dtos.Orders;
using OrderFinanceControl.Dtos.Products;
using OrderFinanceControl.Entities;

namespace OrderFinanceControl.Tests.Integration;

[CollectionDefinition(nameof(IntegrationCollection))]
public sealed class IntegrationCollection : ICollectionFixture<TestMongoFixture>;

[Collection(nameof(IntegrationCollection))]
public class OrderApiIntegrationTests(TestMongoFixture fixture)
{
    private readonly TestMongoFixture _fixture = fixture;

    [Fact]
    public async Task CreateCustomerAndListCustomers_ShouldPersistInMongo()
    {
        var client = _fixture.Factory.CreateClient();

        var createResponse = await client.PostAsJsonAsync("/Customers", new CustomerDto
        {
            Name = "Joao",
            Email = "joao@example.com"
        });

        if (!createResponse.IsSuccessStatusCode)
        {
            var errorBody = await createResponse.Content.ReadAsStringAsync();
            throw new Xunit.Sdk.XunitException($"Create customer failed: {createResponse.StatusCode} - {errorBody}");
        }

        createResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var customersResponse = await client.GetFromJsonAsync<List<CustomerResponseDto>>("/Customers");
        customersResponse.Should().NotBeNull();
        customersResponse!.Should().ContainSingle(c => c.Name == "Joao");

        using var scope = _fixture.Factory.Services.CreateScope();
        var mongoContext = scope.ServiceProvider.GetRequiredService<MongoContext>();
        var persistedCustomer = await mongoContext.Customers.Find(c => c.Email == "joao@example.com").FirstOrDefaultAsync();

        persistedCustomer.Should().NotBeNull();
        persistedCustomer!.Name.Should().Be("Joao");
    }

    [Fact]
    public async Task CreateProductAndCreateOrder_ShouldPersistOrderAndReturnIt()
    {
        var client = _fixture.Factory.CreateClient();

        var customerResponse = await client.PostAsJsonAsync("/Customers", new CustomerDto
        {
            Name = "Maria",
            Email = "maria@example.com"
        });
        if (!customerResponse.IsSuccessStatusCode)
        {
            var errorBody = await customerResponse.Content.ReadAsStringAsync();
            throw new Xunit.Sdk.XunitException($"Create customer failed: {customerResponse.StatusCode} - {errorBody}");
        }

        var productResponse = await client.PostAsJsonAsync("/Product", new ProductDto
        {
            Name = "Mouse",
            Price = 50.0m
        });
        if (!productResponse.IsSuccessStatusCode)
        {
            var errorBody = await productResponse.Content.ReadAsStringAsync();
            throw new Xunit.Sdk.XunitException($"Create product failed: {productResponse.StatusCode} - {errorBody}");
        }
        productResponse.StatusCode.Should().Be(HttpStatusCode.Created);

        using var scope = _fixture.Factory.Services.CreateScope();
        var mongoContext = scope.ServiceProvider.GetRequiredService<MongoContext>();
        var customer = await mongoContext.Customers.Find(c => c.Email == "maria@example.com").FirstAsync();
        var product = await mongoContext.Products.Find(p => p.Name == "Mouse").FirstAsync();

        var orderCreateResponse = await client.PostAsJsonAsync("/Orders", new OrderDto
        {
            CustomerId = customer.Id,
            Items = [new OrderItemDto { ProductId = product.Id, Quantity = 2 }]
        });
        if (!orderCreateResponse.IsSuccessStatusCode)
        {
            var errorBody = await orderCreateResponse.Content.ReadAsStringAsync();
            throw new Xunit.Sdk.XunitException($"Create order failed: {orderCreateResponse.StatusCode} - {errorBody}");
        }
        orderCreateResponse.StatusCode.Should().Be(HttpStatusCode.Created);

        var orderCreated = await orderCreateResponse.Content.ReadFromJsonAsync<Dictionary<string, string>>();
        orderCreated.Should().NotBeNull();
        orderCreated!.Should().ContainKey("id");

        var orderId = orderCreated["id"];
        var orderResponse = await client.GetAsync($"/Orders/{orderId}");
        orderResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var order = await orderResponse.Content.ReadFromJsonAsync<OrderResponseDto>();
        order.Should().NotBeNull();
        order!.Customer.Name.Should().Be("Maria");
        order.Items.Should().ContainSingle();
        order.Items[0].ProductName.Should().Be("Mouse");
        order.TotalAmount.Should().Be(100.0m);

        var persistedOrder = await mongoContext.Orders.Find(o => o.Id == orderId).FirstOrDefaultAsync();
        persistedOrder.Should().NotBeNull();
        persistedOrder!.Items.Should().ContainSingle();
        persistedOrder.TotalAmount.Should().Be(100.0m);
    }
}
