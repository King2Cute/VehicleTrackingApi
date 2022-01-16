using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using VehicleTracking.Core.Persistence;
using VehicleTracking.Helpers;
using VehicleTracking.Models.Requests;
using VehicleTracking.Models.UserDevices;

namespace VehicleTracking.Controllers
{
    public class UserDeviceController : Controller
    {
        public UserDeviceController(IConfiguration config, ILogger<UserDevice> logger, MongoDbService mongoDbService)
        {
            _config = config;
            _logger = logger;
            _mongoDbService = mongoDbService;
            _deviceHelper = new DeviceHelper(_config, _logger, _mongoDbService);
        }

        [Authorize(Roles = "User")]
        [HttpPost]
        [Route("api/device/updateLocation")]
        [SwaggerOperation("UpdateVehicleLocation", Tags = new[] { "Devices" })]
        public async Task<IActionResult> UpdateVehicleLocation([FromBody] LocationUpdateRequest locationUpdate)
        {
            bool vehicleUpdated = await _deviceHelper.UpdateVehicleLocation(GetUserIdFromClaim(), locationUpdate);

            if (!vehicleUpdated)
                return BadRequest();

            return Ok("vehicle location updated for vehicle ID: " + locationUpdate.VehicleId);
        }

        [Authorize(Roles = "User")]
        [HttpPost]
        [Route("api/device/createVehicle")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerOperation("CreateVehicle", Tags = new[] { "Devices" })]
        public async Task<IActionResult> CreateVehicle([FromBody] VehicleRequest vehicleRequest)
        {

            var userDevices = _mongoDbService.UserDevices.AsQueryable().Where(x => x.UserId == Guid.Parse(GetUserIdFromClaim())).FirstOrDefault();

            if (userDevices == null)
                return BadRequest("device not found");

            var device = userDevices.Devices.Where(x => x.Id == vehicleRequest.Vehicle.DeviceId);

            if (device == null)
                return BadRequest("user doesn't own device");

            var vehicleId = await _deviceHelper.CreateVehicle(vehicleRequest);

            if (vehicleId == null)
                return BadRequest("error creating vehicle");

            return Ok(vehicleId);
        }

        [Authorize(Roles = "User")]
        [HttpPost]
        [Route("api/device/{userId}")]
        [SwaggerOperation("CreateDevice", Tags = new[] { "Devices" })]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateDevice([FromRoute] Guid userId)
        {
            if (GetUserIdFromClaim() != userId.ToString())
                return BadRequest(StatusCode(403));

            var deviceId = await _deviceHelper.CreateDevice(userId);

            if (deviceId == null)
                return BadRequest("error creating device");

            return Ok(deviceId);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("api/device/{userId}")]
        [SwaggerOperation("GetDevice", Tags = new[] { "Devices" })]
        public virtual IActionResult GetDevice([FromRoute] Guid userId)
        {
            try
            {
                var getDevice = _deviceHelper.GetDevice(userId);

                if (getDevice == null)
                    return BadRequest("error getting device");

                return Ok(getDevice);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return BadRequest("Error getting device");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("api/device/{deviceId}")]
        [SwaggerOperation("GetDevice", Tags = new[] { "Devices" })]
        public async Task<IActionResult> DeleteDevice([FromRoute] Guid deviceId)
        {
            try
            {
                var deletedDevice = await _deviceHelper.DeleteDevice(Guid.Parse(GetUserIdFromClaim()), deviceId);

                if (!deletedDevice)
                    return BadRequest("error deleting device");

                return Ok(deviceId + " deleted");
            }
            catch(Exception e)
            {
                _logger.LogError(e.Message);
                return BadRequest("Error deleting device");
            }
        }

        private string GetUserIdFromClaim()
        {
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;

            return claims.First().Value;
        }

        private readonly IConfiguration _config;
        private readonly ILogger<UserDevice> _logger;
        private readonly DeviceHelper _deviceHelper;
        private readonly MongoDbService _mongoDbService;
    }
}
