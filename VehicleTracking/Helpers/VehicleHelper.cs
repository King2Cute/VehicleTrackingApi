using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using VehicleTracking.Core.Extensions;
using VehicleTracking.Core.GoogleAPIs;
using VehicleTracking.Core.Persistence;
using VehicleTracking.Models;
using VehicleTracking.Models.GoogleGeoCode;
using VehicleTracking.Models.Vehicles;

namespace VehicleTracking.Helpers
{
    public class VehicleHelper
    {
        public VehicleHelper(IConfiguration config, ILogger<Vehicle> logger, MongoDbService mongoDbService)
        {
            _config = config;
            _logger = logger;
            _mongoDbService = mongoDbService;
            _geoCoding = new GeoCoding(_config, _logger);
        }

        public async Task<Guid?> CreateVehicle(Vehicle vehicle)
        {
            if (vehicle.Id == new Guid())
                vehicle.Id = Guid.NewGuid();

            try
            {
                await _mongoDbService.Vehicles.InsertOneAsync(vehicle);
                return (Guid)vehicle.Id;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error saving vehicle");
            }

            return null;
        }

        public async Task<bool> ReplaceVehicle(Guid vehicleId, Vehicle vehicle)
        {
            var result = await _mongoDbService.Vehicles.ReplaceOneAsync(e => e.Id == vehicleId, vehicle);
            return result.ModifiedCount == 1 ? true : false;
        }

        public Vehicle GetVehicle(Guid vehicleId)
        {
            try
            {
                var vehicle = _mongoDbService.Vehicles.AsQueryable().Where(v => v.Id == vehicleId).First();
                return vehicle;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error getting vehicle");
            }

            return null;
        }

        public async Task<BaseLocation> GetVehiclePosition(Guid vehicleId)
        {
            var now = DateTime.Now;
            var vehicleLocation = _mongoDbService.VehicleLocations.AsQueryable().Where(x => x.VehicleId == vehicleId).First();

            if (vehicleLocation == null)
                return null;

            var times = GetLocationTimes(vehicleLocation.Locations);
            var latestTime = now.GetOrderedTimes(times).Last();

            var location = vehicleLocation.Locations.Where(x => x.Time == latestTime).First();
            location = await _geoCoding.GetGoogleGeoAsync(location);

            return location;
        }

        public async Task<List<BaseLocation>> GetVehiclePositionsFromRange(VehicleRangeRequest timeRange)
        {
            var now = DateTime.Now;
            List<BaseLocation> locationInRange = new List<BaseLocation>();

            try
            {
                var vehicleLocation = _mongoDbService.VehicleLocations.AsQueryable().Where(x => x.VehicleId == timeRange.VehicleId).First();
                foreach (var location in vehicleLocation.Locations)
                {
                    if (location.Time >= timeRange.StartTime && location.Time <= timeRange.EndTime)
                    {
                        var updatedLocation = await _geoCoding.GetGoogleGeoAsync(location);
                        locationInRange.Add(updatedLocation);
                    }
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error getting vehicle location range");
            }

            return locationInRange;
        }

        private List<DateTime> GetLocationTimes(List<BaseLocation> locations)
        {
            List<DateTime> times = new List<DateTime>();
            foreach (var location in locations)
            {
                times.Add(location.Time);
            }

            return times;
        }

        public async Task<bool> DeleteVehicle(Guid vehicleId)
        {
            var result = await _mongoDbService.Vehicles.DeleteOneAsync(v => v.Id == vehicleId);
            return result.DeletedCount == 1 ? true : false;
        }

        private readonly IConfiguration _config;
        private readonly ILogger<Vehicle> _logger;
        private readonly MongoDbService _mongoDbService;
        private readonly GeoCoding _geoCoding;
    }
}
