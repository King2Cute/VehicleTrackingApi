using System;
using System.Threading.Tasks;
using VehicleTracking.Models.Contracts;

namespace VehicleTracking.Core.Persistence
{
    public class WorkUnit : IWorkUnit
    {
        public WorkUnit(IMongoContext context)
        {
            _context = context;
        }

        public async Task<bool> Complete()
        {
            var amount = await _context.SaveChanges();
            return amount > 0;
        }

        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }

        private readonly IMongoContext _context;
    }
}
