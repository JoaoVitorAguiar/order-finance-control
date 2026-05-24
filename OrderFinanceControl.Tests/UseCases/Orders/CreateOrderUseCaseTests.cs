using FluentAssertions;
using Moq;
using OrderFinanceControl.Common.Errors;
using OrderFinanceControl.Data.Repositories.Interfaces;
using OrderFinanceControl.Dtos.Orders;
using OrderFinanceControl.Entities;
using OrderFinanceControl.UseCases.Orders;

namespace OrderFinanceControl.Tests.UseCases.Orders;

public class CreateOrderUseCaseTests
{
    [Fact]
    public async Task ExecuteAsync_WhenCustomerAndProductsExist_ShouldCreateOrder()
    {
        // Arrange
        var customer = new Customer("João", "joao@email.com") { Id = 1 };
        var product1 = new Product("Mouse", 50.0m) { Id = 1 };
        var product2 = new Product("Keyboard", 100.0m) { Id = 2 };

        var customerRepoMock = new Mock<ICustomerRepository>();
        customerRepoMock
            .Setup(r => r.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(customer);

        var productRepoMock = new Mock<IProductRepository>();
        productRepoMock
            .Setup(r => r.GetByIdsAsync(It.IsAny<IEnumerable<int>>()))
            .ReturnsAsync(new List<Product> { product1, product2 });

        var orderRepoMock = new Mock<IOrderRepository>();
        orderRepoMock
            .Setup(r => r.AddAsync(It.IsAny<Order>()))
            .Returns(Task.CompletedTask);

        var useCase = new CreateOrderUseCase(
            orderRepoMock.Object,
            customerRepoMock.Object,
            productRepoMock.Object);

        var dto = new OrderDto
        {
            CustomerId = 1,
            Items = new List<OrderItemDto>
            {
                new() { ProductId = 1, Quantity = 2 },
                new() { ProductId = 2, Quantity = 1 }
            }
        };

        // Act
        var result = await useCase.ExecuteAsync(dto);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeGreaterThan(0);
        orderRepoMock.Verify(r => r.AddAsync(It.IsAny<Order>()), Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_WhenCustomerDoesNotExist_ShouldReturnCustomerNotFoundError()
    {
        // Arrange
        var customerRepoMock = new Mock<ICustomerRepository>();
        customerRepoMock
            .Setup(r => r.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((Customer?)null);

        var productRepoMock = new Mock<IProductRepository>();
        var orderRepoMock = new Mock<IOrderRepository>();

        var useCase = new CreateOrderUseCase(
            orderRepoMock.Object,
            customerRepoMock.Object,
            productRepoMock.Object);

        var dto = new OrderDto
        {
            CustomerId = 999,
            Items = new List<OrderItemDto>
            {
                new() { ProductId = 1, Quantity = 2 }
            }
        };

        // Act
        var result = await useCase.ExecuteAsync(dto);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(OrderErrors.CustomerNotFound);
        orderRepoMock.Verify(r => r.AddAsync(It.IsAny<Order>()), Times.Never);
    }

    [Fact]
    public async Task ExecuteAsync_WhenAnyProductDoesNotExist_ShouldReturnProductsNotFoundError()
    {
        // Arrange
        var customer = new Customer("João", "joao@email.com") { Id = 1 };
        var product1 = new Product("Mouse", 50.0m) { Id = 1 };

        var customerRepoMock = new Mock<ICustomerRepository>();
        customerRepoMock
            .Setup(r => r.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(customer);

        var productRepoMock = new Mock<IProductRepository>();
        productRepoMock
            .Setup(r => r.GetByIdsAsync(It.IsAny<IEnumerable<int>>()))
            .ReturnsAsync(new List<Product> { product1 }); // Apenas 1 produto quando espera 2

        var orderRepoMock = new Mock<IOrderRepository>();

        var useCase = new CreateOrderUseCase(
            orderRepoMock.Object,
            customerRepoMock.Object,
            productRepoMock.Object);

        var dto = new OrderDto
        {
            CustomerId = 1,
            Items = new List<OrderItemDto>
            {
                new() { ProductId = 1, Quantity = 2 },
                new() { ProductId = 999, Quantity = 1 }
            }
        };

        // Act
        var result = await useCase.ExecuteAsync(dto);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(OrderErrors.ProductsNotFound);
        orderRepoMock.Verify(r => r.AddAsync(It.IsAny<Order>()), Times.Never);
    }

    [Fact]
    public async Task ExecuteAsync_WhenOrderCreated_ShouldCalculateTotalAmountCorrectly()
    {
        // Arrange
        var customer = new Customer("João", "joao@email.com") { Id = 1 };
        var product1 = new Product("Mouse", 50.0m) { Id = 1 };
        var product2 = new Product("Keyboard", 100.0m) { Id = 2 };

        var customerRepoMock = new Mock<ICustomerRepository>();
        customerRepoMock
            .Setup(r => r.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(customer);

        var productRepoMock = new Mock<IProductRepository>();
        productRepoMock
            .Setup(r => r.GetByIdsAsync(It.IsAny<IEnumerable<int>>()))
            .ReturnsAsync(new List<Product> { product1, product2 });

        Order? capturedOrder = null;
        var orderRepoMock = new Mock<IOrderRepository>();
        orderRepoMock
            .Setup(r => r.AddAsync(It.IsAny<Order>()))
            .Callback<Order>(o => capturedOrder = o)
            .Returns(Task.CompletedTask);

        var useCase = new CreateOrderUseCase(
            orderRepoMock.Object,
            customerRepoMock.Object,
            productRepoMock.Object);

        var dto = new OrderDto
        {
            CustomerId = 1,
            Items = new List<OrderItemDto>
            {
                new() { ProductId = 1, Quantity = 2 }, // 50 * 2 = 100
                new() { ProductId = 2, Quantity = 1 }  // 100 * 1 = 100
            }
        };

        // Act
        var result = await useCase.ExecuteAsync(dto);

        // Assert
        result.IsSuccess.Should().BeTrue();
        capturedOrder.Should().NotBeNull();
        capturedOrder!.TotalAmount.Should().Be(200.0m);
    }
}
