using System.Text.Json.Serialization;

namespace ShipmentService.API.Data
{
    public class Package
    {
        public int Id { get; set; }

        public double Width { get; set; }
        public double Height { get; set; }
        public double Length { get; set; }
        public double Weight { get; set; }

        public int ShipmentId { get; set; }

        [JsonIgnore]
        public Shipment Shipment { get; set; }
    }
}