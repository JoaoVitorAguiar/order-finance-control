using Microsoft.AspNetCore.Mvc;
using OrderFinanceControl.Data.Repositories.Interfaces;
using OrderFinanceControl.Dtos.Orders;
using OrderFinanceControl.Dtos.Products;
using OrderFinanceControl.UseCases;

namespace OrderFinanceControl.Controllers;

[ApiController]
[Route("[controller]")]
public class OrdersController : ControllerBase
{
    private readonly CreateOrderUseCase _createOrderUseCase;
    private readonly IOrderRepository _orderRepository;
    private readonly MarkOrderAsPaidUseCase _markOrderAsPaidUseCase;
    public OrdersController(
        CreateOrderUseCase createOrderUseCase, 
        IOrderRepository orderRepository,
        MarkOrderAsPaidUseCase markOrderAsPaidUseCase)
    {
        _createOrderUseCase = createOrderUseCase;
        _orderRepository = orderRepository;
        _markOrderAsPaidUseCase = markOrderAsPaidUseCase;
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
        var order = await _orderRepository.GetByIdAsync(id);

        if (order == null)
            return NotFound();

        return Ok(order);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var orders = await _orderRepository.GetAllAsync();

        return Ok(orders);
    }

    [HttpPatch("{id}/pay")]
    public async Task<IActionResult> MarkAsPaid(int id)
    {
        await _markOrderAsPaidUseCase.ExecuteAsync(id);
        return NoContent();
    }
}
