using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VehicleTracking.Core.GoogleAPIs;
using VehicleTracking.Core.Persistence;
using VehicleTracking.Models.GoogleGeoCode;
using VehicleTracking.Models.Requests;
using VehicleTracking.Models.UserDevices;
using VehicleTracking.Models.VehicleLocations;

namespace VehicleTracking.Helpers
{
    public class DeviceHelper
    {
        public DeviceHelper(IConfiguration config, ILogger<UserDevice> logger, MongoDbService mongoDbService)
        {
            _config = config;
            _logger = logger;
            _mongoDbService = mongoDbService;
            _geoCoding = new GeoCoding(_config, _logger);
        }

        public async Task<Guid?> CreateVehicle(VehicleRequest vehicleRequest)
        {
            if (vehicleRequest.Vehicle.DeviceId == Guid.Empty || vehicleRequest.Vehicle.DeviceId == null)
                return null;

            if (vehicleRequest.Vehicle.Id == new Guid())
                vehicleRequest.Vehicle.Id = Guid.NewGuid();

            if (vehicleRequest.VehicleLocation.Id == new Guid())
                vehicleRequest.VehicleLocation.Id = Guid.NewGuid();

            vehicleRequest.VehicleLocation.VehicleId = vehicleRequest.Vehicle.Id;

            try
            {
                await _mongoDbService.Vehicles.InsertOneAsync(vehicleRequest.Vehicle);

                vehicleRequest.VehicleLocation.Locations[0] = await _geoCoding.GetGoogleGeoAsync(vehicleRequest.VehicleLocation.Locations[0]);

                await _mongoDbService.VehicleLocations.InsertOneAsync(vehicleRequest.VehicleLocation);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error saving vehicle");
                return null;
            }

            return vehicleRequest.Vehicle.Id;
        }

        public async Task<bool> UpdateVehicleLocation(string userId, LocationUpdateRequest locationUpdate)
        {
            try
            {
                //grab users devices
                var userDevices = _mongoDbService.UserDevices.AsQueryable().Where(x => x.UserId == Guid.Parse(userId)).FirstOrDefault();

                if (userDevices == null)
                    return false;

                //check to see if the location update is coming from an owned device
                var ownedDevice = userDevices.Devices.Where(x => x.Id == locationUpdate.DeviceId).FirstOrDefault();

                if (ownedDevice == null)
                    throw new UnauthorizedAccessException("vehicle not owned by user");

                var vehicle = _mongoDbService.Vehicles.AsQueryable().Where(v => v.Id == locationUpdate.VehicleId).FirstOrDefault();

                var vehicleLocation = _mongoDbService.VehicleLocations.AsQueryable().Where(v => v.VehicleId == locationUpdate.VehicleId).FirstOrDefault();

                if (vehicleLocation == null)
                    return false;

                locationUpdate.Location = await _geoCoding.GetGoogleGeoAsync(locationUpdate.Location);

                vehicleLocation.Locations.Add(locationUpdate.Location);

                var filter = Builders<VehicleLocation>.Filter.Eq("_id", vehicleLocation.Id);
                var update = Builders<VehicleLocation>.Update.Set("Locations", vehicleLocation.Locations);
                var result = _mongoDbService.VehicleLocations.UpdateOne(filter, update);

                return result.MatchedCount == 1;
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return false;
            }
        }

        public async Task<Guid?> CreateDevice(Guid userId)
        {
            try
            {
                var userDevices = _mongoDbService.UserDevices.AsQueryable().Where(x => x.UserId == userId).FirstOrDefault();

                if (userDevices != null)
                {
                    var device = userDevices.Devices;
                    var newDevice = new Device()
                    {
                        Id = Guid.NewGuid(),
                        CreationDate = DateTime.Now,
                        Online = true
                    };

                    device.Add(newDevice);

                    var filter = Builders<UserDevice>.Filter.Eq("_id", userDevices.Id);
                    var update = Builders<UserDevice>.Update.Set("Devices", device);
                    _mongoDbService.UserDevices.UpdateOne(filter, update);

                    return newDevice.Id;
                }
                else
                {
                    var userDevice = new UserDevice
                    {
                        Id = Guid.NewGuid(),
                        UserId = userId,
                        Devices = new List<Device>
                        {
                            new Device()
                            {
                                Id = Guid.NewGuid(),
                                CreationDate = DateTime.Now,
                                Online = true
                            }
                        }
                    };
                    await _mongoDbService.UserDevices.InsertOneAsync(userDevice);

                    return userDevice.Id;
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error saving vehicle");
            }

            return null;
        }

        public UserDevice GetDevice(Guid userId)
        {
            try
            {
                var userDevice = _mongoDbService.UserDevices.AsQueryable().Where(v => v.UserId == userId).FirstOrDefault();
                return userDevice;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error getting userDevice");
            }

            return null;
        }

        public async Task<bool> DeleteDevice(Guid userId, Guid deviceId)
        {
            var userDevices = _mongoDbService.UserDevices.AsQueryable().Where(x => x.UserId == userId).FirstOrDefault();

            if (userDevices == null)
                return false;

            var devices = userDevices.Devices;

            var device = devices.Where(x => x.Id == deviceId).FirstOrDefault();

            if (device == null)
                return false;

            devices.Remove(device);

            var filter = Builders<UserDevice>.Filter.Eq("_id", userDevices.Id);
            var update = Builders<UserDevice>.Update.Set("Devices", devices);
            var result = await _mongoDbService.UserDevices.UpdateOneAsync(filter, update);

            return result.ModifiedCount == 1;
        }

        private readonly IConfiguration _config;
        private readonly ILogger _logger;
        private readonly MongoDbService _mongoDbService;
        private readonly GeoCoding _geoCoding;
    }
}
