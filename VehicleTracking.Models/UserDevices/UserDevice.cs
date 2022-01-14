using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using VehicleTracking.Models.Contracts;

namespace VehicleTracking.Models.UserDevices
{
    public class UserDevice : IEquatable<UserDevice>, IEntity
    {
        [JsonIgnore]
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public List<Device> Devices { get; set; }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((UserDevice)obj);
        }

        public bool Equals(UserDevice other)
        {
            return other != null &&
                EqualityComparer<Guid?>.Default.Equals(Id, other.Id) &&
                EqualityComparer<Guid?>.Default.Equals(UserId, other.UserId) &&
                Devices == other.Devices;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = 17;
                if (Id != null)
                    hashCode = hashCode * 23 + Id.GetHashCode();
                if (UserId != null)
                    hashCode = hashCode * 23 + UserId.GetHashCode();
                if (Devices != null)
                    hashCode = hashCode * 23 + Devices.GetHashCode();
                return hashCode;
            }
        }

        public static bool operator ==(UserDevice left, UserDevice right)
        {
            return EqualityComparer<UserDevice>.Default.Equals(left, right);
        }

        public static bool operator !=(UserDevice left, UserDevice right)
        {
            return !(left == right);
        }
    }
}
