using FluentAssertions;
using Moq;
using OrderFinanceControl.Data.Repositories.Interfaces;
using OrderFinanceControl.Entities;
using OrderFinanceControl.UseCases.Customers;

namespace OrderFinanceControl.Tests.UseCases.Customers;

public class GetCustomersUseCaseTests
{
    [Fact]
    public async Task ExecuteAsync_WhenCustomersExist_ShouldReturnAllCustomers()
    {
        // Arrange
        var repoMock = new Mock<ICustomerRepository>();
        repoMock
            .Setup(r => r.GetAllAsync())
            .ReturnsAsync([
                new Customer("joao", "joao@email.com"),
                new Customer("maria", "maria@email.com")
            ]);

        var useCase = new GetCustomersUseCase(repoMock.Object);

        // Act
        var customers = await useCase.ExecuteAsync();

        // Assert
        customers.Should().NotBeEmpty();
        customers.Should().HaveCount(2);
    }
}
