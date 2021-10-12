using AutoMapper;
using Intecgra.Cerberus.Domain.Dtos.Auth;
using Intecgra.Cerberus.Domain.Entities;

namespace Intecgra.Cerberus.Domain.Config
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<Client, ClientDto>().ReverseMap();
            CreateMap<ClientApplication, ClientApplicationDto>().ReverseMap();
            CreateMap<UserPermissionDto, UserPermission>().ReverseMap();
            CreateMap<PermissionDto, Permission>().ReverseMap();
            CreateMap<MeDto, UserDto>().ReverseMap();
        }
    }
}