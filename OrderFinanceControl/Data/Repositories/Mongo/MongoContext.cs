using Microsoft.Extensions.Options;
using MongoDB.Driver;
using OrderFinanceControl.Entities;
using OrderFinanceControl.Settings;

namespace OrderFinanceControl.Data.Repositories.Mongo;

public class MongoContext
{
    private readonly IMongoDatabase _database;

    public MongoContext(IOptions<MongoSettings> settings)
    {
        var mongoSettings = settings.Value;

        var client = new MongoClient(mongoSettings.ConnectionString);
        _database = client.GetDatabase(mongoSettings.DatabaseName);
    }

    public IMongoCollection<Customer> Customers =>
        _database.GetCollection<Customer>("customers");

    public IMongoCollection<Product> Products =>
        _database.GetCollection<Product>("products");

    public IMongoCollection<Order> Orders =>
        _database.GetCollection<Order>("orders");
}