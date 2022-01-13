using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VehicleTracking.Core.Extensions;
using VehicleTracking.Core.Persistence;
using VehicleTracking.Models;
using VehicleTracking.Models.Vehicles;

namespace VehicleTracking.Helpers
{
    public class VehicleHelper
    {
        public VehicleHelper(ILogger<Vehicle> logger, MongoDbService mongoDbService)
        {
            _logger = logger;
            _mongoDbService = mongoDbService;
        }

        public async Task<Guid?> CreateVehicle(Vehicle vehicle)
        {
            if (!vehicle.Id.HasValue)
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

        public async Task<bool> ReplaceVehicle(Guid vehicleId, Vehicle vehicle)
        {
            var result = await _mongoDbService.Vehicles.ReplaceOneAsync(e => e.Id.Value == vehicleId, vehicle);
            return result.ModifiedCount == 1 ? true : false;
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

        public Location GetVehiclePosition(Guid vehicleId)
        {
            var now = DateTime.Now;
            var vehicleLocation = _mongoDbService.VehicleLocations.AsQueryable().Where(x => x.Id.Value == vehicleId).First();
            var times = GetLocationTimes(vehicleLocation.Locations);
            var min = now.GetMinTime(times);

            return vehicleLocation.Locations.Where(x => x.Time == min).First();
        }

        public List<Location> GetVehiclePositionsFromRange(Guid vehicleId, DateTime startTime, DateTime endTime)
        {
            var now = DateTime.Now;
            List<Location> locationInRange = new List<Location>();

            try
            {
                var vehicleLocation = _mongoDbService.VehicleLocations.AsQueryable().Where(x => x.Id.Value == vehicleId).First();
                foreach (var location in vehicleLocation.Locations)
                {
                    if (location.Time >= startTime && location.Time <= endTime)
                    {
                        locationInRange.Add(location);
                    }
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error getting vehicle location range");
            }

            return locationInRange;
        }

        private List<DateTime> GetLocationTimes(List<Location> locations)
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
            var result = await _mongoDbService.Vehicles.DeleteOneAsync(v => v.Id.Value == vehicleId);
            return result.DeletedCount == 1 ? true : false;
        }

        private readonly ILogger _logger;
        private readonly MongoDbService _mongoDbService;
    }
}
