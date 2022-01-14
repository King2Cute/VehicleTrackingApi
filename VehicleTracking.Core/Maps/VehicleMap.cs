using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using VehicleTracking.Models.Vehicles;

namespace VehicleTracking.Core.Maps
{
    public class VehicleMap
    {
        public static void Configure()
        {
            BsonClassMap.RegisterClassMap<Vehicle>(map =>
            {
                map.AutoMap();
                map.SetIgnoreExtraElements(true);
                map.MapIdMember(d => d.Id).SetSerializer(new GuidSerializer(GuidRepresentation.Standard));
            });
        }
    }
}
