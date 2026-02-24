namespace OrderFinanceControl.Data.Configurations;

public static class MongoMappings
{
    public static void Register()
    {
        CustomerMongoConfig.Configure();
        ProductMongoConfig.Configure();
        OrderMongoConfig.Configure();
    }
}
