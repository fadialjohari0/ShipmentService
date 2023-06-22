using ShipmentService.API.Models.Shipment;

namespace ShipmentService.API.Models.Users
{
    public class GetUsersDto
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int NumberOfShipments { get; set; }
    }
}