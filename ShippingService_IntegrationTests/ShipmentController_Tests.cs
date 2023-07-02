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
        var token = await _factory.GetJwtTokenAsync();
        var request = new HttpRequestMessage(HttpMethod.Get, "/api/shipments");
        _factory.AddJwtTokenToRequest(request, token);

        // Act
        var response = await _client.SendAsync(request);

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.Equal("application/json; charset=utf-8",
            response.Content.Headers.ContentType?.ToString());
    }

    [Fact]
    public async Task GetShipment_ReturnsShipment_GivenValidId()
    {
        // Arrange
        var token = await _factory.GetJwtTokenAsync();
        var request = new HttpRequestMessage(HttpMethod.Get, "/api/shipments/1");
        _factory.AddJwtTokenToRequest(request, token);
        // Act
        var response = await _client.SendAsync(request);

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
        var token = await _factory.GetJwtTokenAsync();
        var request = new HttpRequestMessage(HttpMethod.Get, "/api/shipments/9999");
        _factory.AddJwtTokenToRequest(request, token);


        // Act
        var response = await _client.SendAsync(request);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}
