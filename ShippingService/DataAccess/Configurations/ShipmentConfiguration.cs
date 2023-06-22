using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ShipmentService.API.Data.Configurations
{
    public class ShipmentConfiguration : IEntityTypeConfiguration<Shipment>
    {
        public void Configure(EntityTypeBuilder<Shipment> builder)
        {
            builder.HasOne<ApiUser>(e => e.ApiUser)
             .WithMany(e => e.UserShipments)
             .HasForeignKey(e => e.UserId);
        }
    }
}