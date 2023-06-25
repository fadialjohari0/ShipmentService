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
using ShipmentService.API.UOW;

namespace ShipmentTestApi.Systems.Controllers
{
    public class TestShipmentController
    {
        private readonly Mock<IUnitOfWork>? _unitOfWorkMock;
        private readonly Mock<IShipmentsRepository>? _shipmentRepositoryMock;
        private readonly Mock<IMapper>? _mapperMock;
        private readonly Mock<PackageDtoValidator>? _packageDtoValidator;
        private readonly Mock<ShipmentDtoValidator>? _shipmentDtoValidator;

        public TestShipmentController()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _shipmentRepositoryMock = new Mock<IShipmentsRepository>();
            _mapperMock = new Mock<IMapper>();
            _packageDtoValidator = new Mock<PackageDtoValidator>();
            _shipmentDtoValidator = new Mock<ShipmentDtoValidator>();

            _unitOfWorkMock.Setup(_ => _.Shipments).Returns(_shipmentRepositoryMock.Object);
        }


        [Fact]
        public async Task GetShipments_ShouldReturn200Status()
        {
            // Arrange
            _shipmentRepositoryMock?.Setup(_ => _.GetDetailsAllAsync()).ReturnsAsync(ShipmentMockData.GetShipments());

            var sut = new ShipmentsController(_unitOfWorkMock?.Object, _mapperMock?.Object, _packageDtoValidator?.Object, _shipmentDtoValidator?.Object);

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
            _shipmentRepositoryMock?.Setup(_ => _.GetDetails(id)).ReturnsAsync(shipment);

            var expectedDto = new GetShipmentDto { Id = id, ShippingCompany = "Fedex" };
            _mapperMock?.Setup(mapper => mapper.Map<GetShipmentDto>(shipment)).Returns(expectedDto);

            var sut = new ShipmentsController(_unitOfWorkMock?.Object, _mapperMock?.Object, _packageDtoValidator?.Object, _shipmentDtoValidator?.Object);

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