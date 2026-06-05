using System.ComponentModel.DataAnnotations;
using FluentAssertions;
using OrderFinanceControl.Dtos.Customers;
using OrderFinanceControl.Dtos.Orders;
using OrderFinanceControl.Dtos.Products;

namespace OrderFinanceControl.Tests.Dtos;

public class DtoValidationTests
{
    [Fact]
    public void CustomerDto_WhenEmailIsInvalid_ShouldFailValidation()
    {
        var dto = new CustomerDto
        {
            Name = "João",
            Email = "invalid-email"
        };

        var results = Validate(dto);

        results.Should().Contain(r => r.MemberNames.Contains(nameof(CustomerDto.Email)));
    }

    [Fact]
    public void ProductDto_WhenPriceIsZero_ShouldFailValidation()
    {
        var dto = new ProductDto
        {
            Name = "Mouse",
            Price = 0
        };

        var results = Validate(dto);

        results.Should().Contain(r => r.MemberNames.Contains(nameof(ProductDto.Price)));
    }

    [Fact]
    public void OrderDto_WhenCustomerIdIsEmpty_ShouldFailValidation()
    {
        var dto = new OrderDto
        {
            CustomerId = string.Empty,
            Items = [new OrderItemDto { ProductId = "prod-1", Quantity = 1 }]
        };

        var results = Validate(dto);

        results.Should().Contain(r => r.MemberNames.Contains(nameof(OrderDto.CustomerId)));
    }

    [Fact]
    public void OrderDto_WhenItemsIsEmpty_ShouldFailValidation()
    {
        var dto = new OrderDto
        {
            CustomerId = "cust-1",
            Items = []
        };

        var results = Validate(dto);

        results.Should().Contain(r => r.MemberNames.Contains(nameof(OrderDto.Items)));
    }

    [Fact]
    public void OrderItemDto_WhenProductIdIsEmpty_ShouldFailValidation()
    {
        var dto = new OrderItemDto
        {
            ProductId = string.Empty,
            Quantity = 1
        };

        var results = Validate(dto);

        results.Should().Contain(r => r.MemberNames.Contains(nameof(OrderItemDto.ProductId)));
    }

    [Fact]
    public void OrderItemDto_WhenQuantityIsZero_ShouldFailValidation()
    {
        var dto = new OrderItemDto
        {
            ProductId = "prod-1",
            Quantity = 0
        };

        var results = Validate(dto);

        results.Should().Contain(r => r.MemberNames.Contains(nameof(OrderItemDto.Quantity)));
    }

    private static List<ValidationResult> Validate(object dto)
    {
        var results = new List<ValidationResult>();
        Validator.TryValidateObject(dto, new ValidationContext(dto), results, validateAllProperties: true);

        return results;
    }
}
