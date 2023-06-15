using System.Threading.Tasks;
using AutoMapper;
using ShipmentTestApi.MockData;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ShipmentService.API.Contracts;
using ShipmentService.API.Controllers;
using ShipmentService.API.Data;
using ShipmentService.API.Models.Shipment;
using ShipmentService.API.Validators;
using Xunit;

namespace ShipmentTestApi.Systems.Controllers
{
    public class TestShipmentController
    {

        private readonly Mock<IShipmentsRepository>? _shipmentRepositoryMock;
        private readonly Mock<IMapper>? _mapperMock;
        private readonly Mock<PackageDtoValidator>? _packageDtoValidator;

        public TestShipmentController()
        {
            _shipmentRepositoryMock = new Mock<IShipmentsRepository>();
            _mapperMock = new Mock<IMapper>();
            _packageDtoValidator = new Mock<PackageDtoValidator>();
        }


        [Fact]
        public async Task GetShipments_ShouldReturn200Status()
        {
            // Arrange
            _shipmentRepositoryMock?.Setup(_ => _.GetDetailsAllAsync()).ReturnsAsync(ShipmentMockData.GetShipments());

            var sut = new ShipmentsController(_mapperMock?.Object, _shipmentRepositoryMock?.Object, _packageDtoValidator?.Object);

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

            var shipment = new Shipment { Id = id, ShipmentId = "Fedex" };
            _shipmentRepositoryMock?.Setup(_ => _.GetDetails(id)).ReturnsAsync(shipment);

            var expectedDto = new GetShipmentDto { Id = id, ShipmentId = "Fedex" };
            _mapperMock?.Setup(mapper => mapper.Map<GetShipmentDto>(shipment)).Returns(expectedDto);

            var sut = new ShipmentsController(_mapperMock?.Object, _shipmentRepositoryMock?.Object, _packageDtoValidator?.Object);

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