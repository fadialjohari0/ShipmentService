using System.Text.Json.Serialization;

namespace ShipmentService.API.Data
{
    public class Shipment
    {
        public int Id { get; set; }
        public string ShippingCompany { get; set; }
        public string ShippingServiceType { get; set; }

        public Package Package { get; set; }

        public string UserId { get; set; }
        [JsonIgnore]
        public ApiUser ApiUser { get; set; }
    }
}