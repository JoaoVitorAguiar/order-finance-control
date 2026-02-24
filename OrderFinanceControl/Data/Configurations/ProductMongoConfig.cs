using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using OrderFinanceControl.Entities;

namespace OrderFinanceControl.Data.Configurations;

public static class ProductMongoConfig
{
    public static void Configure()
    {
        if (BsonClassMap.IsClassMapRegistered(typeof(Product)))
            return;

        BsonClassMap.RegisterClassMap<Product>(cm =>
        {
            cm.AutoMap();

            cm.MapIdProperty(p => p.Id)
              .SetIdGenerator(StringObjectIdGenerator.Instance)
              .SetSerializer(
                  new MongoDB.Bson.Serialization.Serializers.StringSerializer(BsonType.ObjectId)
              );
        });
    }
}