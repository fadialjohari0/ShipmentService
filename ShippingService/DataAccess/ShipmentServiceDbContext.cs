using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShipmentService.API.Data.Configurations;

namespace ShipmentService.API.Data
{
    public class ShipmentServiceDbContext : IdentityDbContext<ApiUser>
    {
        public ShipmentServiceDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Shipment> Shipments { get; set; }
        public DbSet<Package> Packages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new RoleConfiguration());
            modelBuilder.ApplyConfiguration(new ShipmentConfiguration());
            modelBuilder.ApplyConfiguration(new PackageConfiguration());
        }
    }
}