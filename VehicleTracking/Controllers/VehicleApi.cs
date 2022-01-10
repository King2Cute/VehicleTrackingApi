using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Threading.Tasks;
using VehicleTracking.Core.Helpers;
using VehicleTracking.Models;
using VehicleTracking.Models.Vehicles;

namespace VehicleTracking.Controllers
{
    public class VehicleApi : Controller
    {
        public VehicleApi(ILogger<Vehicle> logger)
        {
            _logger = logger;
            _vehicleHelper = new VehicleHelper(_logger);
        }

        [HttpPost]
        [Route("api/vehicle/updateLocation")]
        [SwaggerOperation("UpdateVehicleLocation", Tags = new[] { "Vehicles" })]
        public virtual IActionResult UpdateVehicleLocation(Guid vehicleId, double lat, double lng)
        {
            //database logic

            return new JsonResult(Ok("UpdateVehicleLocation"));
        }

        [HttpGet]
        [Route("api/vehicle/getPosition/{vehicleId}")]
        [SwaggerOperation("UpdateVehicleLocation", Tags = new[] { "Vehicles" })]
        public virtual IActionResult GetVehiclePosition([FromRoute] Guid vehicleId)
        {
            return new JsonResult(Ok("GetVehiclePosition"));
        }

        [HttpGet]
        [Route("api/vehicle/getTimePosition")]
        [SwaggerOperation("GetTimeVehiclePosition", Tags = new[] { "Vehicles" })]
        public virtual IActionResult GetTimeVehiclePosition([FromBody] TimeRange timeRange)
        {
            return new JsonResult(Ok());
        }

        //add auth
        [HttpPost]
        [Route("api/vehicle")]
        [SwaggerOperation("CreateVehicle", Tags = new[] { "Vehicles" })]
        public async virtual Task<IActionResult> CreateVehicle([FromBody] Vehicle vehicle)
        {
            //register vehicle
            var vehicleId = await _vehicleHelper.CreateVehicle(vehicle);

            if (vehicleId == null)
                return BadRequest("error saving vehicle");

            return Ok(vehicleId);
        }

        [HttpPut]
        [Route("api/vehicle/{vehicleId}")]
        [SwaggerOperation("ReplaceVehicle", Tags = new[] { "Vehicles" })]
        public virtual IActionResult ReplaceVehicle([FromRoute] Guid vehicleId, [FromBody] Vehicle vehicle)
        {
            //register vehicle

            //register initial location

            return new JsonResult(Ok());
        }

        [HttpPost]
        [Route("api/vehicle/update")]
        [SwaggerOperation("UpdateVehicle", Tags = new[] { "Vehicles" })]
        public virtual IActionResult UpdateVehicle([FromRoute] Guid vehicleId)
        {
            //register vehicle

            //register initial location

            return new JsonResult(Ok());
        }

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

        private readonly ILogger<Vehicle> _logger;
        private readonly VehicleHelper _vehicleHelper;
    }
}
