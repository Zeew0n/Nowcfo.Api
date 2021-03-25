using AutoMapper;
using Nowcfo.Application.Dtos;
using Nowcfo.Application.Dtos.Role;
using Nowcfo.Application.Dtos.User.Request;
using Nowcfo.Domain.Models;
using Nowcfo.Domain.Models.AppUserModels;

namespace Nowcfo.Application.MappingProfile
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<AppRole, RoleDto>()
                .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.Name)).ReverseMap();
            CreateMap<AppUser,CreateUserDto>().ReverseMap();
            CreateMap<AppUser,UpdateUserDto>().ReverseMap();
            CreateMap<Organization,OrganizationDto>().ReverseMap();
            CreateMap<EmployeeInfo,EmployeeInfoDto>().ReverseMap();
            CreateMap<Designation,DesignationDto>().ReverseMap();
            CreateMap<Permission,PermissionDto>().ReverseMap();
            CreateMap<Menu,MenuDto>().ReverseMap();
        }
    }
}
