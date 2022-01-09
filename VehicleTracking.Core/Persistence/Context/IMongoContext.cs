using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace VehicleTracking.Core.Persistence
{
    interface IMongoContext
    {
        void AddCommand(Func<Task> func);
        IMongoCollection<T> GetCollection<T>(string name);
        Task<int> SaveChanges();
    }
}
