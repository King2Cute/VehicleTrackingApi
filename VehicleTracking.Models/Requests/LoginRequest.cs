using System;
using System.Collections.Generic;
using System.Text;

namespace VehicleTracking.Models.Requests
{
    public class LoginRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
