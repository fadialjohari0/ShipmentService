using ShipmentService.API.Data;
using ShipmentService.API.Contracts;

namespace ShipmentService.API.Repository
{
    public class PackageRepository : GenericRepository<Package>, IPackagesRepository
    {
        private readonly ShipmentServiceDbContext _context;

        public PackageRepository(ShipmentServiceDbContext context) : base(context)
        {
            this._context = context;
        }
    }
}
