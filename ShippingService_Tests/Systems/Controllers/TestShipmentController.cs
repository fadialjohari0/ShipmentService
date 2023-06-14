using System.Threading.Tasks;
using AutoMapper;
using Demo1.TestApi.MockData;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ShipmentService.API.Contracts;
using ShipmentService.API.Controllers;
using ShipmentService.API.Data;
using ShipmentService.API.Models.Shipment;
using ShipmentService.API.Validators;
using Xunit;

namespace Demo1.TestApi.Systems.Controllers
{
    public class TestShipmentController
    {
        [Fact]
        public async Task GetShipments_ShouldReturn200Status()
        {
            // Arrange
            var shipmentsRepositoryMock = new Mock<IShipmentsRepository>();
            shipmentsRepositoryMock.Setup(_ => _.GetDetailsAllAsync()).ReturnsAsync(ShipmentMockData.GetShipments());

            var mapperMock = new Mock<IMapper>();

            PackageDtoValidator packageDtoValidator = new PackageDtoValidator();

            var sut = new ShipmentsController(mapperMock.Object, shipmentsRepositoryMock.Object, packageDtoValidator);

            // Act
            var result = await sut.GetShipments();

            // Assert
            result.Result.Should().BeOfType<OkObjectResult>();
            (result.Result as OkObjectResult)?.StatusCode.Should().Be(200);
        }


        [Fact]
        public async Task GetShipment_ShouldReturnShipment_WhenIdExists()
        {
            // Arrange
            int id = 1;
            var shipmentsRepositoryMock = new Mock<IShipmentsRepository>();
            var mapperMock = new Mock<IMapper>();

            var shipment = new Shipment { Id = id, ShipmentId = "Fedex" };
            shipmentsRepositoryMock.Setup(_ => _.GetDetails(id)).ReturnsAsync(shipment);

            var expectedDto = new GetShipmentDto { Id = id, ShipmentId = "Fedex" };
            mapperMock.Setup(mapper => mapper.Map<GetShipmentDto>(shipment)).Returns(expectedDto);

            PackageDtoValidator packageDtoValidator = new PackageDtoValidator();

            var sut = new ShipmentsController(mapperMock.Object, shipmentsRepositoryMock.Object, packageDtoValidator);

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