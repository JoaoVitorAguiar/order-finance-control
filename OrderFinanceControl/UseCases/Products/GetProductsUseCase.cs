using OrderFinanceControl.Data.Repositories.Interfaces;
using OrderFinanceControl.Entities;

namespace OrderFinanceControl.UseCases.Products;

public class GetProductsUseCase(IProductRepository productRepository)
{
    private readonly IProductRepository _productRepository = productRepository;

    public async Task<IEnumerable<Product>> ExecuteAsync()
    {
        return await _productRepository.GetAllAsync();
    }
}
