using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Conventions;
using VehicleTracking.Core.Maps;

namespace VehicleTracking.Core.Persistence
{
    public class MongoMapping
    {
        public static void Configure(IConfiguration config)
        {
            UserMap.Configure();
            DeviceMap.Configure();
            UserDeviceMap.Configure();
            VehicleMap.Configure();

            var pack = new ConventionPack
            {
                new EnumRepresentationConvention(BsonType.String),
                    new IgnoreExtraElementsConvention(true),
                    new IgnoreIfDefaultConvention(true)
            };
            ConventionRegistry.Register("AppConventions", pack, t => true);
        }
    }
}
