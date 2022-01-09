using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using VehicleTracking.Models.Devices;

namespace VehicleTracking.Controllers
{
    public class DeviceApi : Controller
    {
        [HttpPost]
        [Route("api/device")]
        [SwaggerOperation("CreateDevice", Tags = new[] { "Devices" })]
        public virtual JsonResult CreateDevice([FromBody] Device device)
        {
            return new JsonResult(Ok());
        }

        [HttpPut]
        [Route("api/device/{deviceId}")]
        [SwaggerOperation("ReplaceDevice", Tags = new[] { "Devices" })]
        public virtual JsonResult ReplaceDevice(Guid deviceId, [FromBody] Device device)
        {
            return new JsonResult(Ok());
        }

        [HttpPost]
        [Route("api/device/update")]
        [SwaggerOperation("UpdateDevice", Tags = new[] { "Devices" })]
        public virtual JsonResult UpdateDevice([FromBody] Device device)
        {
            return new JsonResult(Ok());
        }

        [HttpGet]
        [Route("api/device/{deviceId}")]
        [SwaggerOperation("GetDevice", Tags = new[] { "Devices" })]
        public virtual JsonResult GetDevice([FromRoute] string deviceId)
        {
            return new JsonResult(Ok());
        }
    }
}
