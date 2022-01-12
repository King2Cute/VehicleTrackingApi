using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VehicleTracking.Core.Persistence;
using VehicleTracking.Models.Devices;
using VehicleTracking.Models.VehicleLocations;

namespace VehicleTracking.Helpers
{
    public class DeviceHelper
    {
        public DeviceHelper(ILogger<Device> logger, MongoDbService mongoDbService)
        {
            _logger = logger;
            _mongoDbService = mongoDbService;
        }

        public async Task<Guid?> CreateVehicleLocation(VehicleLocation vehicleLocation)
        {
            if (!vehicleLocation.Id.HasValue)
                vehicleLocation.Id = Guid.NewGuid();

            try
            {
                await _mongoDbService.VehicleLocations.InsertOneAsync(vehicleLocation);
                return (Guid)vehicleLocation.Id;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error saving vehicle");
            }

            return null;
        }

        public async Task<Guid?> CreateDevice(Device device)
        {
            if (!device.Id.HasValue)
                device.Id = Guid.NewGuid();

            try
            {
                await _mongoDbService.Devices.InsertOneAsync(device);
                return (Guid)device.Id;
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
