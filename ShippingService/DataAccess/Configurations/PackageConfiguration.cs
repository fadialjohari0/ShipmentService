using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ShipmentService.API.Data.Configurations
{
    public class PackageConfiguration : IEntityTypeConfiguration<Package>
    {
        public void Configure(EntityTypeBuilder<Package> builder)
        {
            builder.HasOne(e => e.Shipment)
             .WithOne(e => e.Package)
             .HasForeignKey<Package>(e => e.ShipmentId)
             .IsRequired();
        }
    }
}