using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ShipmentService.API.Data.Configurations
{
    public class PackageConfiguration : IEntityTypeConfiguration<Package>
    {
        public void Configure(EntityTypeBuilder<Package> builder)
        {
            builder.HasData(
                new Package
                {
                    Id = 1,
                    Width = 10,
                    Height = 10,
                    Length = 10,
                    Weight = 10,
                    ShipmentId = 1
                },

                new Package
                {
                    Id = 2,
                    Width = 20,
                    Height = 20,
                    Length = 20,
                    Weight = 20,
                    ShipmentId = 2
                }
            );

            builder.HasOne(e => e.Shipment)
             .WithOne(e => e.Package)
             .HasForeignKey<Package>(e => e.ShipmentId)
             .IsRequired();
        }
    }
}