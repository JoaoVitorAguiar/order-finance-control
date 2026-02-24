using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Serializers;
using OrderFinanceControl.Entities;
using OrderFinanceControl.Enums;

namespace OrderFinanceControl.Data.Configurations;

public static class OrderMongoConfig
{
    public static void Configure()
    {
        if (BsonClassMap.IsClassMapRegistered(typeof(Order)))
            return;

        BsonClassMap.RegisterClassMap<Order>(cm =>
        {
            cm.AutoMap();

            cm.MapIdProperty(o => o.Id)
              .SetIdGenerator(StringObjectIdGenerator.Instance)
              .SetSerializer(new StringSerializer(BsonType.ObjectId));

            cm.UnmapProperty(o => o.TotalAmount);

            cm.MapProperty(o => o.Status)
              .SetSerializer(new EnumSerializer<OrderStatus>(BsonType.String));

            cm.MapProperty(o => o.CreatedAt)
              .SetSerializer(new DateTimeSerializer(DateTimeKind.Utc));
        });
    }
}