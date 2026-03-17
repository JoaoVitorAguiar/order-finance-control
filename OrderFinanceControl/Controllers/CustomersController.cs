using Microsoft.AspNetCore.Mvc;
using OrderFinanceControl.Common;
using OrderFinanceControl.Dtos.Customers;
using OrderFinanceControl.UseCases.Customers;

namespace OrderFinanceControl.Controllers;

[ApiController]
[Route("[controller]")]
public class CustomersController : ControllerBase
{
    private readonly CreateCustomerUseCase _createCustomerUseCase;
    private readonly GetCustomersUseCase _getCustomersUseCases;

    public CustomersController(CreateCustomerUseCase createCustomerUseCase, GetCustomersUseCase getCustomersUseCases)
    {
        _createCustomerUseCase = createCustomerUseCase;
        _getCustomersUseCases = getCustomersUseCases;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CustomerDto body)
    {
        var result = await _createCustomerUseCase.ExecuteAsync(body);

        return result.Match<IActionResult>(
            value => Ok(value),
            error => Conflict(new ErrorResponse(error.Code, error.Description))
        );
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var customers = await _getCustomersUseCases.ExecuteAsync();
        return Ok(customers);
    }
}
