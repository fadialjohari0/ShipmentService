using ShipmentService.API.Contracts;
using ShipmentService.API.Data;
using ShipmentService.API.Repository;

namespace ShipmentService.API.UOW
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ShipmentServiceDbContext _context;

        public IShipmentsRepository Shipments { get; private set; }
        public IPackagesRepository Packages { get; private set; }
        public IUsersRepository Users { get; private set; }

        public UnitOfWork(ShipmentServiceDbContext context)
        {
            this._context = context;
            Shipments = new ShipmentRepository(_context);
            Packages = new PackageRepository(_context);
            Users = new UserRepository(_context);
        }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}