using Microsoft.AspNetCore.Mvc;
using OrderFinanceControl.Data.Repositories.Interfaces;
using OrderFinanceControl.Dtos.Products;
using OrderFinanceControl.UseCases.Products;

namespace OrderFinanceControl.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductController : ControllerBase
{
    private readonly CreateProductUseCase _createProductUseCase;
    private readonly GetProductsUseCase _getProductsUseCase;
    public ProductController(CreateProductUseCase createProductUseCase, GetProductsUseCase getProductsUseCase)
    {
        _createProductUseCase = createProductUseCase;
        _getProductsUseCase = getProductsUseCase;
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
        var products = await _getProductsUseCase.ExecuteAsync();
        return Ok(products);
    }
}
