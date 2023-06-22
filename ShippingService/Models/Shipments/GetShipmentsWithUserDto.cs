using ShipmentService.API.Models.Users;

namespace ShipmentService.API.Models.Shipment
{
    public class GetShipmentsWithUserDto : BaseShipmentDto
    {
        public int Id { get; set; }
        public string UserId { get; set; }
    }
}