using DeviceManagement_WebApp.Data;
using DeviceManagement_WebApp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace DeviceManagement_WebApp.Repository
{
    public class DeviceRepository : GenericRepository<Device>, IDeviceRepository
    {
        private readonly ConnectedOfficeContext _context;
        public DeviceRepository(ConnectedOfficeContext context) : base(context)
        {
            _context = context;
        }

        public override async Task<IEnumerable<Device>> GetAll()
        {
            return await _context.Set<Device>().Include(d => d.Category).Include(d => d.Zone).ToListAsync();
        }

        public override async Task<Device> FindById(Guid? id)
        {
            if (id == null) return null;

            return await _context.Device
                .Include(d => d.Category)
                .Include(d => d.Zone)
                .FirstOrDefaultAsync(m => m.DeviceId == id);
        }
    }
}
