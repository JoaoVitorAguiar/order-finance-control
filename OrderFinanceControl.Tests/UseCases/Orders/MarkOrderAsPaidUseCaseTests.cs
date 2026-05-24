using FluentAssertions;
using Moq;
using OrderFinanceControl.Common.Errors;
using OrderFinanceControl.Data.Repositories.Interfaces;
using OrderFinanceControl.Entities;
using OrderFinanceControl.Enums;
using OrderFinanceControl.UseCases.Orders;

namespace OrderFinanceControl.Tests.UseCases.Orders;

public class MarkOrderAsPaidUseCaseTests
{
    [Fact]
    public async Task ExecuteAsync_WhenOrderExists_AndStatusIsCreated_ShouldMarkAsPaid()
    {
        // Arrange
        var customer = new Customer("João", "joao@email.com");
        var order = new Order(customer, new List<OrderItem>())
        {
            Id = "order-1",
            Status = OrderStatus.Created
        };

        var repoMock = new Mock<IOrderRepository>();
        repoMock
            .Setup(r => r.GetByIdAsync(It.IsAny<string>()))
            .ReturnsAsync(order);

        var useCase = new MarkOrderAsPaidUseCase(repoMock.Object);

        // Act
        var result = await useCase.ExecuteAsync("order-1");

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeTrue();
        order.Status.Should().Be(OrderStatus.Paid);
        order.PaidAt.Should().NotBeNull();
        repoMock.Verify(r => r.UpdateAsync(It.IsAny<Order>()), Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_WhenOrderDoesNotExist_ShouldReturnNotFoundError()
    {
        // Arrange
        var repoMock = new Mock<IOrderRepository>();
        repoMock
            .Setup(r => r.GetByIdAsync(It.IsAny<string>()))
            .ReturnsAsync((Order?)null);

        var useCase = new MarkOrderAsPaidUseCase(repoMock.Object);

        // Act
        var result = await useCase.ExecuteAsync("missing-order");

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(OrderErrors.NotFound);
        repoMock.Verify(r => r.UpdateAsync(It.IsAny<Order>()), Times.Never);
    }

    [Fact]
    public async Task ExecuteAsync_WhenOrderStatusIsNotCreated_ShouldReturnCannotBePaidError()
    {
        // Arrange
        var customer = new Customer("João", "joao@email.com");
        var order = new Order(customer, new List<OrderItem>())
        {
            Id = "order-1",
            Status = OrderStatus.Paid
        };

        var repoMock = new Mock<IOrderRepository>();
        repoMock
            .Setup(r => r.GetByIdAsync(It.IsAny<string>()))
            .ReturnsAsync(order);

        var useCase = new MarkOrderAsPaidUseCase(repoMock.Object);

        // Act
        var result = await useCase.ExecuteAsync("order-1");

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(OrderErrors.CannotBePaid);
        repoMock.Verify(r => r.UpdateAsync(It.IsAny<Order>()), Times.Never);
    }
}
