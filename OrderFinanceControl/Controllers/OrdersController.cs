using Microsoft.AspNetCore.Mvc;
using OrderFinanceControl.Common;
using OrderFinanceControl.Common.Errors;
using OrderFinanceControl.Dtos.Orders;
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
        GetOrdersUseCase getOrdersUseCase)
    {
        _createOrderUseCase = createOrderUseCase;
        _markOrderAsPaidUseCase = markOrderAsPaidUseCase;
        _getOrderByIdUseCase = getOrderByIdUseCase;
        _getOrdersUseCase = getOrdersUseCase;
    }

    [HttpPost]
    public async Task<IActionResult> Create(OrderDto body)
    {
        var result = await _createOrderUseCase.ExecuteAsync(body);

        return result.Match<IActionResult>(
            orderId => CreatedAtAction(
                nameof(GetById),
                new { id = orderId },
                new { id = orderId }),
            error => NotFound(new ErrorResponse(error.Code, error.Description))
        );
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _getOrderByIdUseCase.ExecuteAsync(id);

        return result.Match<IActionResult>(
            value => Ok(value),
            error => NotFound(new ErrorResponse(error.Code, error.Description))
        );
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
        var result = await _markOrderAsPaidUseCase.ExecuteAsync(id);

        return result.Match<IActionResult>(
            _ => NoContent(),
            error => error == OrderErrors.NotFound
                ? NotFound(new ErrorResponse(error.Code, error.Description))
                : Conflict(new ErrorResponse(error.Code, error.Description))
        );
    }
}
