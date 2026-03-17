using OrderFinanceControl.Common;
using OrderFinanceControl.Common.Errors;
using OrderFinanceControl.Data.Repositories.Interfaces;
using OrderFinanceControl.Dtos.Products;
using OrderFinanceControl.Entities;

namespace OrderFinanceControl.UseCases.Products;

public class CreateProductUseCase(IProductRepository productRepository)
{
    private readonly IProductRepository _productRepository = productRepository;

    public async Task<Result<Product>> ExecuteAsync(ProductDto productDto)
    {
        var existingProduct = await _productRepository.GetByNameAsync(productDto.Name);
        if (existingProduct != null)
        {
            return ProductErrors.NameAlreadyExists;
        }

        var product = new Product(productDto.Name, productDto.Price);
        await _productRepository.AddAsync(product);

        return product;
    }
}
