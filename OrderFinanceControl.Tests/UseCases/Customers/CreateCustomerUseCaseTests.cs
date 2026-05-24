using FluentAssertions;
using Moq;
using OrderFinanceControl.Common.Errors;
using OrderFinanceControl.Data.Repositories.Interfaces;
using OrderFinanceControl.Dtos.Customers;
using OrderFinanceControl.Entities;
using OrderFinanceControl.UseCases.Customers;

namespace OrderFinanceControl.Tests.UseCases.Customers;

public class CreateCustomerUseCaseTests
{
    [Fact]
    public async Task Should_Create_Customer_When_Email_Does_Not_Exist()
    {
        // Arrange
        var repoMock = new Mock<ICustomerRepository>();

        repoMock
            .Setup(r => r.GetByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync((Customer?)null);

        var useCase = new CreateCustomerUseCase(repoMock.Object);

        var dto = new CustomerDto
        {
            Name = "maria",
            Email = "maria@email.com"
        };

        // Act
        var result = await useCase.ExecuteAsync(dto);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();

        repoMock.Verify(r => r.AddAsync(It.IsAny<Customer>()), Times.Once);
    }

    [Fact]
    public async Task Should_Return_Error_When_Customer_With_Same_Email_Exists()
    {
        // Arrange
        var repoMock = new Mock<ICustomerRepository>();
        repoMock
            .Setup(r => r.GetByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync(new Customer("maria", "maria@email.com"));

        var useCase = new CreateCustomerUseCase(repoMock.Object);

        // Act
        var result = await useCase.ExecuteAsync(new CustomerDto
        {
            Name = "maria",
            Email = "maria@email.com"
        });

        // Arrange
        result.IsSuccess.Should().BeFalse();
        result.IsFailure.Should().BeTrue();

        result.Error.Should().Be(CustomerErrors.EmailAlreadyExists);

        repoMock.Verify(r => r.AddAsync(It.IsAny<Customer>()), Times.Never);
    }
}
