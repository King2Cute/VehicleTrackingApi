using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using VehicleTracking.Models.Users;

namespace VehicleTracking.Core.Maps
{
    public class UserMap
    {
        public static void Configure()
        {
            BsonClassMap.RegisterClassMap<User>(map =>
            {
                map.AutoMap();
                map.SetIgnoreExtraElements(true);
                map.MapIdMember(d => d.Id).SetSerializer(new GuidSerializer(GuidRepresentation.Standard));
            });
        }
    }
}
