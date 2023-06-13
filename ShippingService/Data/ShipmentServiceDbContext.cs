using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ShipmentService.API.Data
{
    public class ShipmentServiceDbContext : DbContext
    {
        public ShipmentServiceDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Shipment> Shipments { get; set; }
        public DbSet<Package> Packages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Shipment>().HasData(
                new Shipment
                {
                    Id = 1,
                    ShipmentId = "FedEX",
                    CarrierServiceId = "fedexAIR"
                },

                new Shipment
                {
                    Id = 2,
                    ShipmentId = "FedEX",
                    CarrierServiceId = "fedexGROUND"
                }
            );

            modelBuilder.Entity<Package>().HasData(
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
        }

        public void Configure(EntityTypeBuilder<Package> builder)
        {
            builder.HasOne(e => e.Shipment)
                .WithOne(e => e.Package)
                .HasForeignKey<Package>(e => e.ShipmentId)
                .IsRequired();
        }
    }
}