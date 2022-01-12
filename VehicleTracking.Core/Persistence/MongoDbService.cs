using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using VehicleTracking.Models.Devices;
using VehicleTracking.Models.Vehicles;

namespace VehicleTracking.Core.Persistence
{
    public class MongoDbService
    {
        public MongoDbService(IConfiguration config)
        {
            var settings = MongoClientSettings.FromConnectionString(config.GetConnectionString("MongoConnection"));
            settings.AllowInsecureTls = true;

            var client = new MongoClient(settings);
            var database = client.GetDatabase(config.GetConnectionString("DatabaseName"));

            Vehicles = database.GetCollection<Vehicle>("Vehicles");
            Devices = database.GetCollection<Device>("Devices");
        }

        public IMongoCollection<Vehicle> Vehicles { get; }

        public IMongoCollection<Device> Devices { get; }
    }
}
