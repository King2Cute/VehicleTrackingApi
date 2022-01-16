using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Threading.Tasks;
using VehicleTracking.Core.Persistence;
using VehicleTracking.Helpers;
using VehicleTracking.Models;
using VehicleTracking.Models.Vehicles;

namespace VehicleTracking.Controllers
{
    public class VehicleController : Controller
    {
        public VehicleController(IConfiguration config, ILogger<Vehicle> logger, MongoDbService mongoDbService)
        {
            _config = config;
            _logger = logger;
            _mongoDbService = mongoDbService;
            _vehicleHelper = new VehicleHelper(_config, _logger, _mongoDbService);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("api/vehicle/getPosition/{vehicleId}")]
        [SwaggerOperation("GetVehiclePosition", Tags = new[] { "Vehicles" })]
        public async Task<IActionResult> GetVehiclePosition([FromRoute] Guid vehicleId)
        {
            try
            {
                var vehiclePosition = _vehicleHelper.GetVehiclePosition(vehicleId);
                if (vehiclePosition == null)
                    return BadRequest("can't find vehicle position");

                return Ok(vehiclePosition);
            }
            catch (Exception e)
            {
                _logger.LogError("error getting vehicle position");
                return BadRequest(e.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("api/vehicle/getTimePosition")]
        [SwaggerOperation("GetTimeVehiclePosition", Tags = new[] { "Vehicles" })]
        public IActionResult GetTimeVehiclePosition([FromBody] VehicleRangeRequest timeRange)
        {
            try
            {
                var vehicleRange = _vehicleHelper.GetVehiclePositionsFromRange(timeRange);
                if (vehicleRange == null)
                    return BadRequest("vehicle range is null");

                return Ok(vehicleRange);
            }
            catch (Exception e)
            {
                _logger.LogError("error getting vehicle range");
                return BadRequest(e.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("api/vehicle/delete")]
        [SwaggerOperation("DeleteVehicle", Tags = new[] { "Vehicles" })]
        public virtual IActionResult DeleteVehicle([FromRoute] Guid vehicleId)
        {
            try
            {
                var vehicle = _vehicleHelper.DeleteVehicle(vehicleId);

                if (vehicle == null)
                    return StatusCode(204, "No vehicle found");

                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error deleteing vehicle");
                return StatusCode(400, e.Message);
            }
        }

        [Authorize(Roles = "User")]
        [HttpGet]
        [Route("api/vehicle/{vehicleId}")]
        [SwaggerOperation("GetVehicle", Tags = new[] { "Vehicles" })]
        public virtual IActionResult GetVehicle([FromRoute] Guid vehicleId)
        {
            try
            {
                var vehicle = _vehicleHelper.GetVehicle(vehicleId);

                if (vehicle == null)
                    return StatusCode(204, "No vehicle found");

                return Ok(vehicle);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error getting vehicle");
                return StatusCode(400, e.Message);
            }
        }

        private readonly IConfiguration _config;
        private readonly ILogger<Vehicle> _logger;
        private readonly VehicleHelper _vehicleHelper;
        private readonly MongoDbService _mongoDbService;
    }
}
