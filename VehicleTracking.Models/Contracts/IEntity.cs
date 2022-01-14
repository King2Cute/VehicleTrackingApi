using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;

namespace VehicleTracking.Models.Contracts
{
    public interface IEntity
    {
        Guid Id { get; set; }
    }
}
