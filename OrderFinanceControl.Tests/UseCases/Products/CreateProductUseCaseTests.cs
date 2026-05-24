using FluentAssertions;
using Moq;
using OrderFinanceControl.Data.Repositories.Interfaces;
using OrderFinanceControl.Dtos.Products;
using OrderFinanceControl.Entities;
using OrderFinanceControl.UseCases.Products;

namespace OrderFinanceControl.Tests.UseCases.Products;

public class CreateProductUseCaseTests
{
    [Theory]
    [InlineData("Mouse", 10.0)]
    [InlineData("Keyboard", 20.5)]
    [InlineData("Monitor", 150.99)]
    public async Task Should_Create_Product_When_Name_Does_Not_Exist(string name, decimal price)
    {
        // Arrange
        var repoMock = new Mock<IProductRepository>();
        repoMock
            .Setup(r => r.GetByNameAsync(It.IsAny<string>()))
            .ReturnsAsync((Product?)null);

        var useCase = new CreateProductUseCase(repoMock.Object);

        var dto = new ProductDto
        {
            Name = name,
            Price = price
        };

        // Act
        var result = await useCase.ExecuteAsync(dto);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        repoMock.Verify(r => r.AddAsync(It.IsAny<Product>()), Times.Once);
    }

    [Fact]
    public async Task Should_Return_Error_When_Product_With_Same_Name_Exists()
    {
        // Arrange
        var repoMock = new Mock<IProductRepository>();
        repoMock
            .Setup(r => r.GetByNameAsync(It.IsAny<string>()))
            .ReturnsAsync(new Product("Mouse", 10.0m));
        var useCase = new CreateProductUseCase(repoMock.Object);

        // Act
        var result = await useCase.ExecuteAsync(new ProductDto
        {
            Name = "Mouse",
            Price = 10.0m
        });
        // Assert
        result.IsSuccess.Should().BeFalse();
    }
}
