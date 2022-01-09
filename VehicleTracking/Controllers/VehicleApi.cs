using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using VehicleTracking.Models;
using VehicleTracking.Models.Vehicles;

namespace VehicleTracking.Controllers
{
    public class VehicleApi : Controller
    {
        [HttpPost]
        [Route("api/vehicle/updateLocation")]
        [SwaggerOperation("UpdateVehicleLocation", Tags = new[] { "Vehicles" })]
        public virtual JsonResult UpdateVehicleLocation(Guid vehicleId, double lat, double lng)
        {
            //database logic

            return new JsonResult(Ok("UpdateVehicleLocation"));
        }

        [HttpGet]
        [Route("api/vehicle/getPosition/{vehicleId}")]
        [SwaggerOperation("UpdateVehicleLocation", Tags = new[] { "Vehicles" })]
        public virtual JsonResult GetVehiclePosition([FromRoute] Guid vehicleId)
        {
            return new JsonResult(Ok("GetVehiclePosition"));
        }

        [HttpGet]
        [Route("api/vehicle/getTimePosition")]
        [SwaggerOperation("GetTimeVehiclePosition", Tags = new[] { "Vehicles" })]
        public virtual JsonResult GetTimeVehiclePosition([FromBody] TimeRange timeRange)
        {
            return new JsonResult(Ok());
        }

        //add auth
        [HttpPost]
        [Route("api/vehicle")]
        [SwaggerOperation("CreateVehicle", Tags = new[] { "Vehicles" })]
        public virtual JsonResult CreateVehicle([FromBody] Vehicle vehicle)
        {
            //register vehicle

            //register initial location

            return new JsonResult(Ok());
        }

        [HttpPut]
        [Route("api/vehicle/{vehicleId}")]
        [SwaggerOperation("ReplaceVehicle", Tags = new[] { "Vehicles" })]
        public virtual JsonResult ReplaceVehicle([FromRoute] Guid vehicleId, [FromBody] Vehicle vehicle)
        {
            //register vehicle

            //register initial location

            return new JsonResult(Ok());
        }

        [HttpPost]
        [Route("api/vehicle/update")]
        [SwaggerOperation("UpdateVehicle", Tags = new[] { "Vehicles" })]
        public virtual JsonResult UpdateVehicle([FromRoute] Guid vehicleId)
        {
            //register vehicle

            //register initial location

            return new JsonResult(Ok());
        }

        [HttpGet]
        [Route("api/vehicle/{vehicleId}")]
        [SwaggerOperation("GetVehicle", Tags = new[] { "Vehicles" })]
        public virtual JsonResult GetVehicle([FromRoute] string vehicleId)
        {
            //get vehicle
            return new JsonResult(Ok());
        }
    }
}
