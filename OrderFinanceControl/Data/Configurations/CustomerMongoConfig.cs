using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using OrderFinanceControl.Entities;

namespace OrderFinanceControl.Data.Configurations;

public static class CustomerMongoConfig
{
    public static void Configure()
    {
        if (BsonClassMap.IsClassMapRegistered(typeof(Customer)))
            return;

        BsonClassMap.RegisterClassMap<Customer>(cm =>
        {
            cm.AutoMap();

            cm.MapIdProperty(c => c.Id)
              .SetIdGenerator(StringObjectIdGenerator.Instance)
              .SetSerializer(new MongoDB.Bson.Serialization.Serializers.StringSerializer(BsonType.ObjectId));
        });
    }
}