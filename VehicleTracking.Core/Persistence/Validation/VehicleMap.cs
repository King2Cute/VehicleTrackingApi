using MongoDB.Bson.Serialization;
using VehicleTracking.Models.Vehicles;

namespace VehicleTracking.Core.Persistence.Validation
{
    public class VehicleMap
    {
        public static void Configure()
        {
            BsonClassMap.RegisterClassMap<Vehicle>(map =>
            {
                map.AutoMap();
                map.SetIgnoreExtraElements(true);
                map.MapIdMember(x => x.Id);
            });
        }
    }
}
