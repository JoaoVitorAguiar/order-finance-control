using Microsoft.AspNetCore.Mvc;
using OrderFinanceControl.Data.Repositories.Interfaces;
using OrderFinanceControl.Dtos.Customers;
using OrderFinanceControl.Entities;
using OrderFinanceControl.UseCases;

namespace OrderFinanceControl.Controllers;

[ApiController]
[Route("customers")]
public class CustomersController : ControllerBase
{
    private readonly ICustomerRepository _customerRepository;
    private readonly CreateCustomerUseCase _createCustomerUseCase;

    public CustomersController(ICustomerRepository customerRepository, CreateCustomerUseCase createCustomerUseCase)
    {
        _createCustomerUseCase = createCustomerUseCase;
        _customerRepository = customerRepository;
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
        var customers = await _customerRepository.GetAllAsync();
        return Ok(customers);
    }
}