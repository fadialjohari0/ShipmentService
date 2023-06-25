using System.Net;
using System.Threading.Tasks;
using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System.Net.Http;
using ShipmentService.API.Models.Shipment;

public class ShipmentsControllerTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory _factory;

    public ShipmentsControllerTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });
    }

    [Fact]
    public async Task GetAllShipments_ReturnsSuccessCodeAndCorrectContentType()
    {
        // Arrange
        var url = "/api/shipments";

        // Act
        var response = await _client.GetAsync(url);

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.Equal("application/json; charset=utf-8",
            response.Content.Headers.ContentType.ToString());
    }

    [Fact]
    public async Task GetShipment_ReturnsShipment_GivenValidId()
    {
        // Arrange
        var url = "/api/shipments/1";

        // Act
        var response = await _client.GetAsync(url);

        // Assert
        response.EnsureSuccessStatusCode();
        var stringResponse = await response.Content.ReadAsStringAsync();
        var model = JsonConvert.DeserializeObject<GetShipmentDto>(stringResponse);

        Assert.NotNull(model);
        Assert.Equal(1, model.Id);
        Assert.Equal("TestCompany1", model.ShippingCompany);
        Assert.Equal("TestServiceType1", model.ShippingServiceType);
    }

    [Fact]
    public async Task GetShipment_ReturnsNotFound_GivenInvalidId()
    {
        // Arrange
        var url = "/api/shipments/9999";

        // Act
        var response = await _client.GetAsync(url);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}
