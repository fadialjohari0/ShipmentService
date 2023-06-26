using Xunit;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using ShipmentTestApi.MockData;
using ShipmentService.API.Models.Shipment;
using ShipmentTestApi.Builders;
using System.Threading.Tasks;
using Moq;
using ShipmentService.API.Data;

namespace ShipmentTestApi.Systems.Controllers
{
    public class TestShipmentController
    {
        [Fact]
        public async Task GetShipments_ShouldReturn200Status()
        {
            // Arrange
            var builder = new ShipmentsControllerBuilder();
            builder.ShipmentRepositoryMock.Setup(_ => _.GetDetailsAllAsync()).ReturnsAsync(ShipmentMockData.GetShipments());

            var sut = builder.Build();

            // Act
            var result = await sut.GetAllShipments();

            // Assert
            result.Result.Should().BeOfType<OkObjectResult>();
            (result.Result as OkObjectResult)?.StatusCode.Should().Be(200);
        }

        [Fact]
        public async Task GetShipment_ShouldReturnShipment_WhenIdExists()
        {
            // Arrange
            int id = 1;

            var shipment = new Shipment { Id = id, ShippingCompany = "Fedex" };
            var builder = new ShipmentsControllerBuilder();
            builder.ShipmentRepositoryMock.Setup(_ => _.GetDetails(id)).ReturnsAsync(shipment);

            var expectedDto = new GetShipmentDto { Id = id, ShippingCompany = "Fedex" };
            builder.MapperMock.Setup(mapper => mapper.Map<GetShipmentDto>(shipment)).Returns(expectedDto);

            var sut = builder.Build();

            // Act
            var result = await sut.GetShipment(id);

            // Assert
            result.Result.Should().BeOfType<OkObjectResult>();
            (result.Result as OkObjectResult)?.StatusCode.Should().Be(200);

            var actualDto = (result.Result as OkObjectResult)?.Value as GetShipmentDto;
            actualDto.Should().BeEquivalentTo(expectedDto);
        }
    }
}
