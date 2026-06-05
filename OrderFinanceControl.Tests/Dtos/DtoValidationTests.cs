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
    public void OrderDto_WhenCustomerIdIsZero_ShouldFailValidation()
    {
        var dto = new OrderDto
        {
            CustomerId = 0,
            Items = [new OrderItemDto { ProductId = 1, Quantity = 1 }]
        };

        var results = Validate(dto);

        results.Should().Contain(r => r.MemberNames.Contains(nameof(OrderDto.CustomerId)));
    }

    [Fact]
    public void OrderDto_WhenItemsIsEmpty_ShouldFailValidation()
    {
        var dto = new OrderDto
        {
            CustomerId = 1,
            Items = []
        };

        var results = Validate(dto);

        results.Should().Contain(r => r.MemberNames.Contains(nameof(OrderDto.Items)));
    }

    [Fact]
    public void OrderItemDto_WhenProductIdIsZero_ShouldFailValidation()
    {
        var dto = new OrderItemDto
        {
            ProductId = 0,
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
            ProductId = 1,
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
