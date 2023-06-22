using ShipmentService.API.Contracts;

namespace ShipmentService.API.UOW
{
    public interface IUnitOfWork : IDisposable
    {
        IShipmentsRepository Shipments { get; }
        IPackagesRepository Packages { get; }
        IUsersRepository Users { get; }
        Task<int> CompleteAsync();
    }
}