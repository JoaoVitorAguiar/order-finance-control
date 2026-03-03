using OrderFinanceControl.Data.Repositories.Interfaces;
using OrderFinanceControl.Dtos.Products;
using OrderFinanceControl.Entities;
using OrderFinanceControl.Exceptions;

namespace OrderFinanceControl.UseCases.Products;

public class CreateProductUseCase(IProductRepository productRepository)
{
    private readonly IProductRepository _productRepository = productRepository;
    public async Task ExecuteAsync(ProductDto productDto)
    {
        var existingProduct = await _productRepository.GetByNameAsync(productDto.Name);
        if (existingProduct != null)
        {
            throw new AlreadyExistsException("A product with the same name already exists.");
        }
        var product = new Product(productDto.Name, productDto.Price);
        await _productRepository.AddAsync(product);
    }
}
