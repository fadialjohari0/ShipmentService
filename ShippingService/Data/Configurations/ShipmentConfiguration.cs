using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ShipmentService.API.Data.Configurations
{
    public class ShipmentConfiguration : IEntityTypeConfiguration<Shipment>
    {
        public void Configure(EntityTypeBuilder<Shipment> builder)
        {
            builder.HasData(
                new Shipment
                {
                    Id = 1,
                    ShippingCompany = "FedEX",
                    ShippingServiceType = "fedexAIR"
                },

                new Shipment
                {
                    Id = 2,
                    ShippingCompany = "FedEX",
                    ShippingServiceType = "fedexGROUND"
                }
            );
        }
    }
}