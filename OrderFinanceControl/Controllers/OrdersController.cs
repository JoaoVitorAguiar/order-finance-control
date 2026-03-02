using Microsoft.AspNetCore.Mvc;
using OrderFinanceControl.Data.Repositories.Interfaces;
using OrderFinanceControl.Dtos.Orders;
using OrderFinanceControl.Dtos.Products;
using OrderFinanceControl.UseCases.Orders;

namespace OrderFinanceControl.Controllers;

[ApiController]
[Route("[controller]")]
public class OrdersController : ControllerBase
{
    private readonly GetOrderByIdUseCase _getOrderByIdUseCase;
    private readonly CreateOrderUseCase _createOrderUseCase;
    private readonly MarkOrderAsPaidUseCase _markOrderAsPaidUseCase;
    private readonly GetOrdersUseCase _getOrdersUseCase;
    public OrdersController(
        CreateOrderUseCase createOrderUseCase, 
        MarkOrderAsPaidUseCase markOrderAsPaidUseCase,
        GetOrderByIdUseCase getOrderByIdUseCase,
        GetOrdersUseCase getOrdersUseCase   )
    {
        _createOrderUseCase = createOrderUseCase;
        _markOrderAsPaidUseCase = markOrderAsPaidUseCase;
        _getOrderByIdUseCase = getOrderByIdUseCase;
        _getOrdersUseCase = getOrdersUseCase;
    }

    [HttpPost]
    public async Task<IActionResult> Create(OrderDto body)
    {
        var orderId =  await _createOrderUseCase.ExecuteAsync(body);
        return CreatedAtAction(
        nameof(GetById),      
        new { id = orderId },
        new { id = orderId }  
        );
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var order = await _getOrderByIdUseCase.ExecuteAsync(id);

        if (order == null)
            return NotFound();

        return Ok(order);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var orders = await _getOrdersUseCase.ExecuteAsync();

        return Ok(orders);
    }

    [HttpPatch("{id}/pay")]
    public async Task<IActionResult> MarkAsPaid(int id)
    {
        await _markOrderAsPaidUseCase.ExecuteAsync(id);
        return NoContent();
    }
}
