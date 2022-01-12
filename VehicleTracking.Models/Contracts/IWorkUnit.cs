using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace VehicleTracking.Models.Contracts
{
    public interface IWorkUnit : IDisposable
    {
        Task<bool> Complete();
    }
}
