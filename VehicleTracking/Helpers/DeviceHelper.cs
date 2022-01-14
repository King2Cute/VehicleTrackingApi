﻿using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VehicleTracking.Core.Persistence;
using VehicleTracking.Models.Requests;
using VehicleTracking.Models.UserDevices;
using VehicleTracking.Models.VehicleLocations;

namespace VehicleTracking.Helpers
{
    public class DeviceHelper
    {
        public DeviceHelper(ILogger<UserDevice> logger, MongoDbService mongoDbService)
        {
            _logger = logger;
            _mongoDbService = mongoDbService;
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
                await _mongoDbService.VehicleLocations.InsertOneAsync(vehicleRequest.VehicleLocation);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error saving vehicle");
                return null;
            }

            return vehicleRequest.Vehicle.Id;
        }

        public bool UpdateVehicleLocation(string userId, LocationUpdateRequest locationUpdate)
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

        private readonly ILogger _logger;
        private readonly MongoDbService _mongoDbService;
    }
}
