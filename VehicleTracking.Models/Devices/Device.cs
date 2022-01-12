using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using VehicleTracking.Models.Contracts;

namespace VehicleTracking.Models.Devices
{
    public class Device : Location, IEquatable<Device>, IEntity
    {
        public Guid? Id { get; set; }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((Device)obj);
        }


        public bool Equals(Device other)
        {
            return other != null &&
                EqualityComparer<Guid?>.Default.Equals(Id, other.Id);
        }

        public static bool operator ==(Device left, Device right)
        {
            return EqualityComparer<Device>.Default.Equals(left, right);
        }

        public static bool operator !=(Device left, Device right)
        {
            return !(left == right);
        }
    }
}
