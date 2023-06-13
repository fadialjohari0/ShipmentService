using System.ComponentModel.DataAnnotations.Schema;

namespace ShipmentService.API.Data
{
    public class Shipment
    {
        public int Id { get; set; }
        public string ShipmentId { get; set; }
        public string CarrierServiceId { get; set; }

        public Package Package { get; set; }
    }
}