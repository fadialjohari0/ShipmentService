using ShipmentService.API.Models.Package;

namespace ShipmentService.API.Models.Shipment
{
    public class BaseShipmentDto
    {
        public string ShippingCompany { get; set; }
        public string ShippingServiceType { get; set; }
        public PackageDto Package { get; set; }
    }
}