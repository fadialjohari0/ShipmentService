using ShipmentService.API.Models.Package;

namespace ShipmentService.API.Models.Shipment
{
    public class BaseShipmentDto
    {
        public string ShipmentId { get; set; }
        public string CarrierServiceId { get; set; }
        public PackageDto Package { get; set; }
    }
}