using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VehicleTracking.Core.Persistence;
using VehicleTracking.Models.Devices;

namespace VehicleTracking.Helpers
{
    public class DeviceHelper
    {
        public DeviceHelper(ILogger<Device> logger, MongoDbService mongoDbService)
        {
            _logger = logger;
            _mongoDbService = mongoDbService;
        }

        private readonly ILogger _logger;
        private readonly MongoDbService _mongoDbService;
    }
}
