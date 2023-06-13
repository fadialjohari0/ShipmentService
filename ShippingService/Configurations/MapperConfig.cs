using AutoMapper;
using ShipmentService.API.Data;
using ShipmentService.API.Models.Package;
using ShipmentService.API.Models.Shipment;

namespace ShipmentService.API.Configurations
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {
            CreateMap<Shipment, CreateShipmentDto>().ReverseMap();
            CreateMap<Shipment, GetShipmentDto>().ReverseMap();

            CreateMap<Package, PackageDto>().ReverseMap();
        }
    }
}