using ShipmentService.API.Models.Shipment;

namespace ShipmentService.API.Models.Users
{
    public class GetUsersShipmentsDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public List<BaseShipmentDto> UserShipments { get; set; }
    }
}