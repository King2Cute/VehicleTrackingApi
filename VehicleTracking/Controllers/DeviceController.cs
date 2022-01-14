using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Threading.Tasks;
using VehicleTracking.Core.Persistence;
using VehicleTracking.Helpers;
using VehicleTracking.Models;
using VehicleTracking.Models.Devices;
using VehicleTracking.Models.Requests;
using VehicleTracking.Models.VehicleLocations;
using VehicleTracking.Models.Vehicles;

namespace VehicleTracking.Controllers
{
    public class DeviceController : Controller
    {
        public DeviceController(ILogger<Device> logger, MongoDbService mongoDbService)
        {
            _logger = logger;
            _mongoDbService = mongoDbService;
            _deviceHelper = new DeviceHelper(_logger, _mongoDbService);
        }

        [HttpPost]
        [Route("api/device/updateLocation")]
        [SwaggerOperation("UpdateVehicleLocation", Tags = new[] { "Devices" })]
        public virtual IActionResult UpdateVehicleLocation([FromBody] LocationUpdateRequest locationUpdate)
        {
            bool vehicleUpdated = _deviceHelper.UpdateVehicleLocation(locationUpdate);

            if (!vehicleUpdated)
                return BadRequest();


            return Ok("vehicle location updated for vehicle ID: " + locationUpdate.VehicleId);
        }

        //assuming the device is the one needing to do the initial creation of vehicle and position
        [HttpPost]
        [Route("api/device/createVehicle")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerOperation("CreateVehicle", Tags = new[] { "Devices" })]
        public virtual IActionResult CreateVehicle([FromBody] VehicleRequest vehicleRequest)
        {
            var vehicleId = _deviceHelper.CreateVehicle(vehicleRequest);

            if (vehicleId == null)
                return BadRequest("error creating vehicle");

            return Ok(vehicleId);
        }

        [HttpPost]
        [Route("api/device")]
        [SwaggerOperation("CreateDevice", Tags = new[] { "Devices" })]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateDevice()
        {
            var deviceId = await _deviceHelper.CreateDevice();

            if (deviceId == null)
                return BadRequest("error creating device");

            return Ok(deviceId);
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
