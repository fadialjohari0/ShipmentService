using ShipmentService.API.Data;
using ShipmentService.API.Contracts;
using Microsoft.EntityFrameworkCore;

namespace ShipmentService.API.Repository
{
    public class UserRepository : GenericRepository<ApiUser>, IUsersRepository
    {
        private readonly ShipmentServiceDbContext _context;

        public UserRepository(ShipmentServiceDbContext context) : base(context)
        {
            this._context = context;
        }

        public async Task<List<ApiUser>> GetDetailsAllAsync()
        {
            return await _context.Users.Include(q => q.UserShipments).ToListAsync();
        }
    }
}
