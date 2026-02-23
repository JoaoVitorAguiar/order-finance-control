using OrderFinanceControl.Data.Repositories.Interfaces;
using OrderFinanceControl.Dtos.Orders;
using OrderFinanceControl.Entities;
using OrderFinanceControl.Exceptions;

namespace OrderFinanceControl.UseCases;

public class CreateOrderUseCase
{
    private readonly IOrderRepository _orderRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly IProductRepository _productRepository;

    public CreateOrderUseCase(
        IOrderRepository orderRepository,
        ICustomerRepository customerRepository,
        IProductRepository productRepository)
    {
        _orderRepository = orderRepository;
        _customerRepository = customerRepository;
        _productRepository = productRepository;
    }

    public async Task<int> ExecuteAsync(OrderDto dto)
    {
        var customerExists = await _customerRepository.GetByIdAsync(dto.CustomerId);
        if (customerExists == null)
            throw new NotFoundException("Customer not found.");

        var productIds = dto.Items.Select(i => i.ProductId).Distinct();
        var products = await _productRepository.GetByIdsAsync(productIds);

        if (products.Count() != productIds.Count())
            throw new NotFoundException("One or more products not found.");

        var productsDict = products.ToDictionary(p => p.Id);

        var items = dto.Items
            .Select(i => new OrderItem(
                i.ProductId,
                productsDict[i.ProductId].Price,
                i.Quantity))
            .ToList();

        var order = new Order(dto.CustomerId, items);

        await _orderRepository.AddAsync(order);
        await _orderRepository.SaveChangesAsync();

        return order.Id;
    }
}