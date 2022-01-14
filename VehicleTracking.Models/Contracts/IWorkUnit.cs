using System;
using System.Threading.Tasks;

namespace VehicleTracking.Models.Contracts
{
    public interface IWorkUnit : IDisposable
    {
        Task<bool> Complete();
    }
}
