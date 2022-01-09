using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VehicleTracking.Models.Devices
{
    public class Device : Location
    {
        public Guid Id { get; set; }
    }
}
