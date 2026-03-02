using Microsoft.AspNetCore.Mvc;
using OrderFinanceControl.Data.Repositories.Interfaces;
using OrderFinanceControl.Dtos.Customers;
using OrderFinanceControl.Entities;
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
        await _createCustomerUseCase.ExecuteAsync(body);

        return Created();
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var customers = await _getCustomersUseCases.ExecuteAsync();
        return Ok(customers);
    }
}