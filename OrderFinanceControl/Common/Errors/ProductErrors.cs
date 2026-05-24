using OrderFinanceControl.Common;

namespace OrderFinanceControl.Common.Errors;

public static class ProductErrors
{
    public static readonly Error NameAlreadyExists =
        new("PRODUCT_NAME_EXISTS", "A product with the same name already exists.");
}
