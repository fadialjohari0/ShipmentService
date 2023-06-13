using ShipmentService.API.Data;
using ShipmentService.API.Contracts;
using Microsoft.EntityFrameworkCore;

namespace ShipmentService.API.Repository
{
    public class ShipmentRepository : GenericRepository<Shipment>, IShipmentsRepository
    {
        private readonly ShipmentServiceDbContext _context;

        public ShipmentRepository(ShipmentServiceDbContext context) : base(context)
        {
            this._context = context;
        }

        public async Task<Shipment> GetDetails(int id)
        {
            return await _context.Shipments.Include(q => q.Package)
             .FirstOrDefaultAsync(q => q.Id == id);
        }

        public async Task<List<Shipment>> GetDetailsAllAsync()
        {
            return await _context.Shipments.Include(q => q.Package).ToListAsync();
        }
    }
}
