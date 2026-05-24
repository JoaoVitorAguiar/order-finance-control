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
        var customer1 = new Customer("João", "joao@email.com");
        var customer2 = new Customer("Maria", "maria@email.com");
        var product = new Product("Mouse", 50.0m);

        var orders = new List<Order>
        {
            new Order(customer1.Id, new List<OrderItem>())
            {
                Id = 1,
                Customer = customer1,
                TotalAmount = 100.0m,
                Items = new List<OrderItem>
                {
                    new OrderItem(product.Id, 50.0m, 2) { Product = product }
                }
            },
            new Order(customer2.Id, new List<OrderItem>())
            {
                Id = 2,
                Customer = customer2,
                TotalAmount = 75.0m,
                Items = new List<OrderItem>
                {
                    new OrderItem(product.Id, 25.0m, 3) { Product = product }
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
        result.First().Id.Should().Be(1);
        result.Last().Id.Should().Be(2);
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
