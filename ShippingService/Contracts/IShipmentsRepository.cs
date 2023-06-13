using ShipmentService.API.Data;

namespace ShipmentService.API.Contracts
{
    public interface IShipmentsRepository : IGenericRepository<Shipment>
    {
        Task<Shipment> GetDetails(int id);
        Task<List<Shipment>> GetDetailsAllAsync();
    }
}
