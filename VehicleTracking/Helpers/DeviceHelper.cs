using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using System;
using System.Linq;
using System.Threading.Tasks;
using VehicleTracking.Core.Persistence;
using VehicleTracking.Models;
using VehicleTracking.Models.Devices;
using VehicleTracking.Models.Requests;
using VehicleTracking.Models.VehicleLocations;
using VehicleTracking.Models.Vehicles;

namespace VehicleTracking.Helpers
{
    public class DeviceHelper
    {
        public DeviceHelper(ILogger<Device> logger, MongoDbService mongoDbService)
        {
            _logger = logger;
            _mongoDbService = mongoDbService;
        }

        public async Task<Guid?> CreateVehicle(VehicleRequest vehicleRequest)
        {
            if (!vehicleRequest.Vehicle.Id.HasValue)
                vehicleRequest.Vehicle.Id = Guid.NewGuid();

            if (!vehicleRequest.VehicleLocation.Id.HasValue)
                vehicleRequest.VehicleLocation.Id = Guid.NewGuid();

            vehicleRequest.VehicleLocation.VehicleId = vehicleRequest.Vehicle.Id.Value;
            vehicleRequest.Vehicle.DeviceId = vehicleRequest.DeviceId;

            try
            {
                await _mongoDbService.Vehicles.InsertOneAsync(vehicleRequest.Vehicle);
            }
            catch(Exception e)
            {
                _logger.LogError(e, "Error saving vehicle");
                return null;
            }

            try
            {
                await _mongoDbService.VehicleLocations.InsertOneAsync(vehicleRequest.VehicleLocation);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error saving vehicle location");
                return null;
            }
            return vehicleRequest.Vehicle.Id.Value;
        }

        public bool UpdateVehicleLocation(LocationUpdateRequest locationUpdate)
        {
            var vehicle = _mongoDbService.Vehicles.AsQueryable().Where(v => v.Id == locationUpdate.VehicleId).First();

            if (vehicle.DeviceId != locationUpdate.DeviceId)
                throw new UnauthorizedAccessException();

            var vehicleLocations = _mongoDbService.VehicleLocations.AsQueryable().Where(v => v.VehicleId == locationUpdate.VehicleId).First().Locations;

            if (vehicleLocations == null)
                return false;

            try
            {
                vehicleLocations.Add(locationUpdate.Location);

                var filter = Builders<VehicleLocation>.Filter.Eq("_id", locationUpdate.VehicleId);
                var update = Builders<VehicleLocation>.Update.Set("Locations", vehicleLocations);
                var result = _mongoDbService.VehicleLocations.UpdateOne(filter, update);

                return result.MatchedCount == 1 ? true : false;
            }
            catch(Exception e)
            {
                _logger.LogError(e.Message);
                return false;
            }
        }

        public async Task<Guid?> CreateDevice()
        {

            try
            {
                var device = new Device
                {
                    Id = Guid.NewGuid(),
                    CreationDate = DateTime.Now,
                    Online = true
                };
                await _mongoDbService.Devices.InsertOneAsync(device);
                return device.Id.Value;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error saving vehicle");
            }

            return null;
        }



        private readonly ILogger _logger;
        private readonly MongoDbService _mongoDbService;
    }
}
