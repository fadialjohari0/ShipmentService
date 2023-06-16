namespace ShipmentService.API.Data
{
    public class Shipment
    {
        public int Id { get; set; }
        public string ShippingCompany { get; set; }
        public string ShippingServiceType { get; set; }

        public Package Package { get; set; }
    }
}