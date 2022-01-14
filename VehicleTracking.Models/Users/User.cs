using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using VehicleTracking.Models.Contracts;
using VehicleTracking.Models.Vehicles;

namespace VehicleTracking.Models.Users
{
    public class User : IEquatable<User>, IEntity
    {
        [JsonIgnore]
        public Guid? Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        [JsonIgnore]
        public List<string> UserRoles { get; set; }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((User)obj);
        }

        public bool Equals(User other)
        {
            return other != null &&
                EqualityComparer<Guid?>.Default.Equals(Id, other.Id) &&
                Email == other.Email &&
                Password == other.Password &&
                UserRoles == other.UserRoles;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = 17;
                if (Id != null)
                    hashCode = hashCode * 23 + Id.GetHashCode();
                if (Email != null)
                    hashCode = hashCode * 23 + Email.GetHashCode();
                if (Password != null)
                    hashCode = hashCode * 23 + Password.GetHashCode();
                if (UserRoles != null)
                    hashCode = hashCode * 23 + UserRoles.GetHashCode();
                return hashCode;
            }
        }

        public static bool operator ==(User left, User right)
        {
            return EqualityComparer<User>.Default.Equals(left, right);
        }

        public static bool operator !=(User left, User right)
        {
            return !(left == right);
        }
    }
}
