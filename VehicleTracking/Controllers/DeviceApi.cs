using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;
using System;
using VehicleTracking.Core.Persistence;
using VehicleTracking.Helpers;
using VehicleTracking.Models.Devices;

namespace VehicleTracking.Controllers
{
    public class DeviceApi : Controller
    {
        public DeviceApi(ILogger<Device> logger, MongoDbService mongoDbService)
        {
            _logger = logger;
            _mongoDbService = mongoDbService;
            _deviceHelper = new DeviceHelper(_logger, _mongoDbService);
        }

        [HttpPost]
        [Route("api/device")]
        [SwaggerOperation("CreateDevice", Tags = new[] { "Devices" })]
        public virtual IActionResult CreateDevice([FromBody] Device device)
        {
            return new JsonResult(Ok());
        }

        [HttpPut]
        [Route("api/device/{deviceId}")]
        [SwaggerOperation("ReplaceDevice", Tags = new[] { "Devices" })]
        public virtual IActionResult ReplaceDevice(Guid deviceId, [FromBody] Device device)
        {
            return new JsonResult(Ok());
        }

        [HttpPost]
        [Route("api/device/update")]
        [SwaggerOperation("UpdateDevice", Tags = new[] { "Devices" })]
        public virtual IActionResult UpdateDevice([FromBody] Device device)
        {
            return new JsonResult(Ok());
        }

        [HttpGet]
        [Route("api/device/{deviceId}")]
        [SwaggerOperation("GetDevice", Tags = new[] { "Devices" })]
        public virtual IActionResult GetDevice([FromRoute] string deviceId)
        {
            return new JsonResult(Ok());
        }

        private readonly ILogger<Device> _logger;
        private readonly DeviceHelper _deviceHelper;
        private readonly MongoDbService _mongoDbService;
    }
}
