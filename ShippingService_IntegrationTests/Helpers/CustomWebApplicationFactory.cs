using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ShipmentService.API.Data;


public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    public async Task<string> GetJwtTokenAsync()
    {
        var token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJmYWRpQGV4YW1wbGUuY29tIiwianRpIjoiMDc2MTJmMDktMTdhZS00MWU4LTk1OWItNzQyNTFmYTY3ZDdkIiwiaWQiOiI3MzE5ODhjNS1kOTAyLTRjNjYtOGRkNi0xMWM2YTYwYzA2NjciLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJBZG1pbmlzdHJhdG9yIiwiZXhwIjoxNjg4MzQyNjIwLCJpc3MiOiJTaGlwbWVudFNlcnZpY2VBUEkiLCJhdWQiOiJTaGlwbWVudFNlcnZpY2VBUElDbGllbnQifQ.TVUEsuXwGvmUlVAfp65HZfFXrg-ab66JC5qrHkCTzvs";
        return await Task.FromResult(token);
    }

    public void AddJwtTokenToRequest(HttpRequestMessage request, string token)
    {
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }

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
