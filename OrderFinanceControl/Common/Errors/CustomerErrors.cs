using OrderFinanceControl.Common;

namespace OrderFinanceControl.Common.Errors;

public static class CustomerErrors
{
    public static readonly Error EmailAlreadyExists =
        new("CUSTOMER_EMAIL_EXISTS", "A customer with the same email already exists.");

    public static readonly Error NotFound =
        new("CUSTOMER_NOT_FOUND", "Customer not found");
}
