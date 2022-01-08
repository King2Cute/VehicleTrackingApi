using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VehicleTracking.Controllers
{
    public class VehicleApi : Controller
    {
        //add auth
        [HttpPost]
        [Route("api/vehicle")]
        public virtual JsonResult CreateVehicle()
        {
            //register vehicle

            //register initial location

            return new JsonResult(Ok());
        }

        [HttpPut]
        [Route("api/vehicle")]
        public virtual JsonResult ReplaceVehicle(Guid id)
        {
            //register vehicle

            //register initial location

            return new JsonResult(Ok());
        }

        [HttpPost]
        [Route("api/vehicle")]
        public virtual JsonResult UpdateVehicle(Guid id)
        {
            //register vehicle

            //register initial location

            return new JsonResult(Ok());
        }

        [HttpGet]
        [Route("api/vehicle/{id}")]
        public virtual JsonResult GetVehicle()
        {
            //get vehicle
            return new JsonResult(Ok());
        }
    }
}
