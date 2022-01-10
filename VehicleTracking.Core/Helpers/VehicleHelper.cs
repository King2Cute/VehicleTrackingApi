using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using System;
using System.Linq;
using System.Threading.Tasks;
using VehicleTracking.Core.Persistence;
using VehicleTracking.Models.Vehicles;

namespace VehicleTracking.Core.Helpers
{
    public class VehicleHelper
    {
        public VehicleHelper(ILogger<Vehicle> logger)
        {
            _logger = logger;
        }

        public async Task<Guid?> CreateVehicle(Vehicle vehicle)
        {
            if (vehicle.Id.HasValue)
                vehicle.Id = Guid.NewGuid();

            try
            {
                await _mongoDbService.Vehicles.InsertOneAsync(vehicle);
                return (Guid)vehicle.Id;
            }
            catch(Exception e)
            {
                _logger.LogError(e, "Error saving vehicle");
            }

            return null;
        }

        public Vehicle GetVehicle(Guid vehicleId)
        {
            try 
            {
                var vehicle = _mongoDbService.Vehicles.AsQueryable().Where(v => v.Id.Value == vehicleId).First();
                return vehicle;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error getting vehicle");
            }

            return null;
        }

        private readonly ILogger _logger;
        private readonly MongoDbService _mongoDbService;
    }
}
