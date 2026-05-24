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
        var customer = new Customer("João", "joao@email.com");
        var product = new Product("Mouse", 50.0m);
        var orderItem = new OrderItem(product.Id, 50.0m, 2);

        var order = new Order(customer.Id, new List<OrderItem> { orderItem })
        {
            Id = 1,
            Customer = customer,
            Items = new List<OrderItem>
            {
                new OrderItem(product.Id, 50.0m, 2) { Id = 1, Product = product }
            }
        };

        var repoMock = new Mock<IOrderRepository>();
        repoMock
            .Setup(r => r.GetByIdWithDetailsAsync(It.IsAny<int>()))
            .ReturnsAsync(order);

        var useCase = new GetOrderByIdUseCase(repoMock.Object);

        // Act
        var result = await useCase.ExecuteAsync(1);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.Id.Should().Be(1);
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
            .Setup(r => r.GetByIdWithDetailsAsync(It.IsAny<int>()))
            .ReturnsAsync((Order?)null);

        var useCase = new GetOrderByIdUseCase(repoMock.Object);

        // Act
        var result = await useCase.ExecuteAsync(999);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(OrderErrors.NotFound);
    }
}
