using System.Collections.Generic;
using ShipmentService.API.Data;

namespace ShipmentTestApi.MockData
{
    public class ShipmentMockData
    {
        public static List<Shipment> GetShipments()
        {
            return new List<Shipment>
            {
                new Shipment
                {
                    Id = 1,
                    ShippingCompany = "Fedex",
                    ShippingServiceType = "fedexAIR",
                    Package = new Package
                    {
                        Width = 5,
                        Height = 6,
                        Length = 14,
                        Weight = 10
                    }
                },

                new Shipment
                {
                    Id = 2,
                    ShippingCompany = "UPS",
                    ShippingServiceType = "UPS2DAY",
                    Package = new Package
                    {
                        Width = 7,
                        Height = 3,
                        Length = 6,
                        Weight = 5
                    }
                },

                new Shipment
                {
                    Id = 3,
                    ShippingCompany = "UPS",
                    ShippingServiceType = "UPSExpress",
                    Package = new Package
                    {
                        Width = 4,
                        Height = 8,
                        Length = 12,
                        Weight = 15
                    }
                }
            };
        }
    }
}
