using Microsoft.AspNetCore.Mvc;
using System;

namespace VehicleTracking.Controllers
{
    public class DeviceApi : Controller
    {
        [HttpPost]
        [Route("api/device")]
        public virtual JsonResult CreateDevice()
        {
            return new JsonResult(Ok());
        }

        [HttpPut]
        [Route("api/device")]
        public virtual JsonResult ReplaceDevice(Guid id)
        {
            return new JsonResult(Ok());
        }

        [HttpPost]
        [Route("api/device")]
        public virtual JsonResult UpdateDevice(Guid id)
        {
            return new JsonResult(Ok());
        }

        [HttpGet]
        [Route("api/device/{id}")]
        public virtual JsonResult GetDevice()
        {
            return new JsonResult(Ok());
        }
    }
}
