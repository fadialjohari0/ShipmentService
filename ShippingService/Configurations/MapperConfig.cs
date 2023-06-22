using AutoMapper;
using ShipmentService.API.Data;
using ShipmentService.API.Models.Package;
using ShipmentService.API.Models.Shipment;
using ShipmentService.API.Models.Users;

namespace ShipmentService.API.Configurations
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {
            CreateMap<Shipment, CreateShipmentDto>().ReverseMap();
            CreateMap<Shipment, GetShipmentDto>().ReverseMap();
            CreateMap<GetShipmentsWithUserDto, Shipment>().ReverseMap();
            CreateMap<BaseShipmentDto, Shipment>().ReverseMap();

            CreateMap<Package, PackageDto>().ReverseMap();
            CreateMap<Package, GetAllPackagesDto>().ReverseMap();

            CreateMap<ApiUserDto, ApiUser>().ReverseMap();

            CreateMap<ApiUser, GetUsersDto>()
                               .ForMember(dest => dest.NumberOfShipments,
                                   opt => opt.MapFrom(src => src.UserShipments.Count));

            CreateMap<ApiUser, GetUsersShipmentsDto>()
                           .ForMember(dest => dest.UserShipments,
                               opt => opt.MapFrom(src => src.UserShipments));
        }
    }
}