using FluentAssertions;
using Moq;
using OrderFinanceControl.Data.Repositories.Interfaces;
using OrderFinanceControl.Entities;
using OrderFinanceControl.UseCases.Orders;

namespace OrderFinanceControl.Tests.UseCases.Orders;

public class GetOrdersUseCaseTests
{
    [Fact]
    public async Task ExecuteAsync_WhenOrdersExist_ShouldReturnAllOrders()
    {
        // Arrange
        var customer1 = new Customer("João", "joao@email.com") { Id = "customer-1" };
        var customer2 = new Customer("Maria", "maria@email.com") { Id = "customer-2" };
        var product = new Product("Mouse", 50.0m) { Id = "product-1" };

        var orders = new List<Order>
        {
            new Order(customer1, new List<OrderItem>())
            {
                Id = "order-1",
                Items = new List<OrderItem>
                {
                    new(product.Id, product.Name, 50.0m, 2)
                }
            },
            new Order(customer2, new List<OrderItem>())
            {
                Id = "order-2",
                Items = new List<OrderItem>
                {
                    new(product.Id, product.Name, 25.0m, 3)
                }
            }
        };

        var repoMock = new Mock<IOrderRepository>();
        repoMock
            .Setup(r => r.GetAllAsync())
            .ReturnsAsync(orders);

        var useCase = new GetOrdersUseCase(repoMock.Object);

        // Act
        var result = await useCase.ExecuteAsync();

        // Assert
        result.Should().NotBeEmpty();
        result.Should().HaveCount(2);
        result.First().Id.Should().Be("order-1");
        result.Last().Id.Should().Be("order-2");
    }

    [Fact]
    public async Task ExecuteAsync_WhenNoOrdersExist_ShouldReturnEmptyList()
    {
        // Arrange
        var repoMock = new Mock<IOrderRepository>();
        repoMock
            .Setup(r => r.GetAllAsync())
            .ReturnsAsync(new List<Order>());

        var useCase = new GetOrdersUseCase(repoMock.Object);

        // Act
        var result = await useCase.ExecuteAsync();

        // Assert
        result.Should().BeEmpty();
    }
}
