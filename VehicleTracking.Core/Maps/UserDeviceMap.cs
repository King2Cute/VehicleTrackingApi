using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using VehicleTracking.Models.UserDevices;

namespace VehicleTracking.Core.Maps
{
    public class UserDeviceMap
    {
        public static void Configure()
        {
            BsonClassMap.RegisterClassMap<UserDevice>(map =>
            {
                map.AutoMap();
                map.SetIgnoreExtraElements(true);
                map.MapIdMember(d => d.Id).SetSerializer(new GuidSerializer(GuidRepresentation.Standard));
            });
        }
    }
}
