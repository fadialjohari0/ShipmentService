using System;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ShipmentService.API.Contracts;
using ShipmentService.API.Data;
using ShipmentService.API.Repository;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType ==
                    typeof(DbContextOptions<ShipmentServiceDbContext>));

            if (descriptor != null)
            {
                services.Remove(descriptor);
            }

            services.AddDbContext<ShipmentServiceDbContext>(options =>
            {
                options.UseInMemoryDatabase("InMemoryAppDb");
            });

            var sp = services.BuildServiceProvider();

            using (var scope = sp.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var db = scopedServices.GetRequiredService<ShipmentServiceDbContext>();

                db.Database.EnsureCreated();

                try
                {
                    InitializeDbForTests(db);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred seeding the database with test messages. Error: {ex.Message}");
                }
            }
        });
    }

    private void InitializeDbForTests(ShipmentServiceDbContext db)
    {
        db.Shipments.AddRange(
            new Shipment
            {
                Id = 1,
                UserId = "TestUser1",
                ShippingCompany = "TestCompany1",
                ShippingServiceType = "TestServiceType1",
                Package = new Package
                {
                    Width = 10,
                    Height = 10,
                    Length = 10,
                    Weight = 10,
                }
            },
            new Shipment
            {
                Id = 2,
                UserId = "TestUser2",
                ShippingCompany = "TestCompany2",
                ShippingServiceType = "TestServiceType2",
                Package = new Package
                {
                    Width = 20,
                    Height = 20,
                    Length = 20,
                    Weight = 20,
                }
            }
        );

        db.SaveChanges();
    }
}
