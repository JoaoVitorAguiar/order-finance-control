using Castle.Core.Resource;
using FluentAssertions;
using Moq;
using OrderFinanceControl.Data.Repositories.Interfaces;
using OrderFinanceControl.Entities;
using OrderFinanceControl.UseCases.Products;

namespace OrderFinanceControl.Tests.UseCases.Products;

public class GetProductsUseCaseTests
{
    [Fact]
    public async Task ExecuteAsync_WhenProductsExist_ShouldReturnAllProducts()
    {
        // Arrange
        var repoMock = new Mock<IProductRepository>();
        repoMock
            .Setup(x => x.GetAllAsync())
            .ReturnsAsync(new List<Product>
            {
                new ("Product 1", 10.0m),
                new ("Product 2", 20.0m)
            });
        var useCase = new GetProductsUseCase(repoMock.Object);

        // Act
        var products = await useCase.ExecuteAsync();

        // Assert
        products.Should().NotBeEmpty();
        products.Should().HaveCount(2);
    }
}
