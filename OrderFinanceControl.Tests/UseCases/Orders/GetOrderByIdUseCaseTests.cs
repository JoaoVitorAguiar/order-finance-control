using FluentAssertions;
using Moq;
using OrderFinanceControl.Common.Errors;
using OrderFinanceControl.Data.Repositories.Interfaces;
using OrderFinanceControl.Entities;
using OrderFinanceControl.Enums;
using OrderFinanceControl.UseCases.Orders;

namespace OrderFinanceControl.Tests.UseCases.Orders;

public class GetOrderByIdUseCaseTests
{
    [Fact]
    public async Task ExecuteAsync_WhenOrderExists_ShouldReturnOrderWithDetails()
    {
        // Arrange
        var customer = new Customer("João", "joao@email.com") { Id = "customer-1" };
        var product = new Product("Mouse", 50.0m) { Id = "product-1" };

        var order = new Order(customer, new List<OrderItem>
        {
            new(product.Id, product.Name, 50.0m, 2)
        })
        {
            Id = "order-1"
        };

        var repoMock = new Mock<IOrderRepository>();
        repoMock
            .Setup(r => r.GetByIdWithDetailsAsync(It.IsAny<string>()))
            .ReturnsAsync(order);

        var useCase = new GetOrderByIdUseCase(repoMock.Object);

        // Act
        var result = await useCase.ExecuteAsync("order-1");

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.Id.Should().Be("order-1");
        result.Value.TotalAmount.Should().Be(100.0m);
        result.Value.Customer.Name.Should().Be("João");
        result.Value.Items.Should().HaveCount(1);
    }

    [Fact]
    public async Task ExecuteAsync_WhenOrderDoesNotExist_ShouldReturnNotFoundError()
    {
        // Arrange
        var repoMock = new Mock<IOrderRepository>();
        repoMock
            .Setup(r => r.GetByIdWithDetailsAsync(It.IsAny<string>()))
            .ReturnsAsync((Order?)null);

        var useCase = new GetOrderByIdUseCase(repoMock.Object);

        // Act
        var result = await useCase.ExecuteAsync("missing-order");

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(OrderErrors.NotFound);
    }
}
