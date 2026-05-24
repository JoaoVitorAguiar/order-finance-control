using Microsoft.AspNetCore.Mvc;
using OrderFinanceControl.Common;
using OrderFinanceControl.Common.Errors;
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
        var result = await _createProductUseCase.ExecuteAsync(body);

        return result.Match<IActionResult>(
            value => CreatedAtAction(nameof(GetAll), new { id = value.Id }, value),
            error => error == ProductErrors.NameAlreadyExists
                ? Conflict(new ErrorResponse(error.Code, error.Description))
                : BadRequest(new ErrorResponse(error.Code, error.Description))
        );
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var products = await _getProductsUseCase.ExecuteAsync();
        return Ok(products);
    }
}
