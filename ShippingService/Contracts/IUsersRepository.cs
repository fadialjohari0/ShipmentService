using ShipmentService.API.Data;

namespace ShipmentService.API.Contracts
{
    public interface IUsersRepository : IGenericRepository<ApiUser>
    {
        Task<List<ApiUser>> GetDetailsAllAsync();
    }
}
