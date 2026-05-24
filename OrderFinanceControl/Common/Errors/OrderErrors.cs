using OrderFinanceControl.Common;

namespace OrderFinanceControl.Common.Errors;

public static class OrderErrors
{
    public static readonly Error CustomerNotFound =
        new("ORDER_CUSTOMER_NOT_FOUND", "Customer not found.");

    public static readonly Error ProductsNotFound =
        new("ORDER_PRODUCTS_NOT_FOUND", "One or more products not found.");

    public static readonly Error NotFound =
        new("ORDER_NOT_FOUND", "Order not found.");

    public static readonly Error CannotBePaid =
        new("ORDER_CANNOT_BE_PAID", "Order cannot be paid.");
}
