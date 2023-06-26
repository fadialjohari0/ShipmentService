using Moq;
using ShipmentService.API.Contracts;
using ShipmentService.API.Controllers;
using ShipmentService.API.Validators;
using ShipmentService.API.UOW;
using AutoMapper;

namespace ShipmentTestApi.Builders
{
    public class ShipmentsControllerBuilder
    {
        public Mock<IUnitOfWork> UnitOfWorkMock { get; private set; }
        public Mock<IShipmentsRepository> ShipmentRepositoryMock { get; private set; }
        public Mock<IMapper> MapperMock { get; private set; }
        public Mock<PackageDtoValidator> PackageDtoValidatorMock { get; private set; }
        public Mock<ShipmentDtoValidator> ShipmentDtoValidatorMock { get; private set; }

        public ShipmentsControllerBuilder()
        {
            UnitOfWorkMock = new Mock<IUnitOfWork>();
            ShipmentRepositoryMock = new Mock<IShipmentsRepository>();
            MapperMock = new Mock<IMapper>();
            PackageDtoValidatorMock = new Mock<PackageDtoValidator>();
            ShipmentDtoValidatorMock = new Mock<ShipmentDtoValidator>();

            UnitOfWorkMock.Setup(_ => _.Shipments).Returns(ShipmentRepositoryMock.Object);
        }

        public ShipmentsController Build()
        {
            return new ShipmentsController(
                UnitOfWorkMock.Object,
                MapperMock.Object,
                PackageDtoValidatorMock.Object,
                ShipmentDtoValidatorMock.Object
            );
        }
    }
}
