using Microsoft.EntityFrameworkCore;
using OrderFinanceControl.Data.Repositories.Interfaces;
using OrderFinanceControl.Dtos.Customers;
using OrderFinanceControl.Dtos.Orders;
using OrderFinanceControl.Entities;

namespace OrderFinanceControl.Data.Repositories.Sql;

public class OrderSqlRepository(OrderFinanceControlDbContext dbContext) : IOrderRepository
{
    private readonly OrderFinanceControlDbContext _dbContext = dbContext;
    public async Task AddAsync(Order order)
    {
        await _dbContext.Orders.AddAsync(order);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<OrderResponseDto>> GetAllAsync()
    {
        return await _dbContext.Orders
            .AsNoTracking()
            .Select(o => new OrderResponseDto
            {
                Id = o.Id,

                Customer = new CustomerResponseDto
                {
                    Id = o.Customer.Id,
                    Name = o.Customer.Name
                },

                CreatedAt = o.CreatedAt,
                TotalAmount = o.TotalAmount,
                Status = o.Status.ToString(),

                Items = o.Items.Select(i => new OrderItemResponseDto
                {
                    ProductId = i.ProductId,
                    ProductName = i.Product.Name,
                    Quantity = i.Quantity,
                    UnitPriceAtOrderTime = i.UnitPriceAtOrderTime
                }).ToList()
            })
            .ToListAsync();
    }

    public async Task<OrderResponseDto?> GetByIdAsync(int id)
    {
        return await _dbContext.Orders
            .AsNoTracking()
            .Select(o => new OrderResponseDto
            {
                Id = o.Id,

                Customer = new CustomerResponseDto
                {
                    Id = o.Customer.Id,
                    Name = o.Customer.Name
                },

                CreatedAt = o.CreatedAt,
                TotalAmount = o.TotalAmount,
                Status = o.Status.ToString(),

                Items = o.Items.Select(i => new OrderItemResponseDto
                {
                    ProductId = i.ProductId,
                    ProductName = i.Product.Name,
                    Quantity = i.Quantity,
                    UnitPriceAtOrderTime = i.UnitPriceAtOrderTime
                }).ToList()
            })
            .FirstOrDefaultAsync(o => o.Id == id);
    }

    public Task<Order?> GetEntityByIdAsync(int id)
    {
        return _dbContext.Orders.FirstOrDefaultAsync(o => o.Id == id);
    }

    public async Task UpdateAsync(Order order)
    {
        _dbContext.Orders.Update(order);
        await _dbContext.SaveChangesAsync();
    }
}
