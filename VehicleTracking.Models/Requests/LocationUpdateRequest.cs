﻿using System;

namespace VehicleTracking.Models.Requests
{
    public class LocationUpdateRequest
    {
        public Guid VehicleId { get; set; }
        public Guid DeviceId { get; set; }
        public Location Location { get; set; }
    }
}
