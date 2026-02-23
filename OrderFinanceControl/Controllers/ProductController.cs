using Microsoft.AspNetCore.Mvc;
using OrderFinanceControl.Data.Repositories.Interfaces;
using OrderFinanceControl.Dtos.Products;
using OrderFinanceControl.UseCases;

namespace OrderFinanceControl.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductController : ControllerBase
{
    private readonly CreateProductUseCase _createProductUseCase;
    private readonly IProductRepository _productRepository;
    public ProductController(CreateProductUseCase createProductUseCase, IProductRepository productRepository)
    {
        _createProductUseCase = createProductUseCase;
        _productRepository = productRepository;
    }

    [HttpPost]
    public async Task<IActionResult> Create(ProductDto body)
    {
        await _createProductUseCase.ExecuteAsync(body);
        return Created();
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var products = await _productRepository.GetAllAsync();
        return Ok(products);
    }
}
