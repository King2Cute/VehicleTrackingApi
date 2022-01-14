using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using VehicleTracking.Models.Contracts;

namespace VehicleTracking.Models.UserDevices
{
    public class Device : IEquatable<Device>, IEntity
    {
        [IgnoreDataMember]
        public Guid Id { get; set; }
        public bool Online { get; set; }
        public DateTime CreationDate { get; set; }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((Device)obj);
        }

        public bool Equals(Device other)
        {
            return other != null &&
                EqualityComparer<Guid?>.Default.Equals(Id, other.Id) &&
                Online == other.Online &&
                EqualityComparer<DateTime>.Default.Equals(CreationDate, other.CreationDate);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = 17;
                if (Id != null)
                    hashCode = hashCode * 23 + Id.GetHashCode();
                if (CreationDate != null)
                    hashCode = hashCode * 23 + CreationDate.GetHashCode();
                return hashCode;
            }
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
