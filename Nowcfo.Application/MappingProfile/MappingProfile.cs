﻿using AutoMapper;
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
            CreateMap<EmployeePermission, EmployeePermissionDto>().ReverseMap();
            CreateMap<Designation,DesignationDto>().ReverseMap();
            CreateMap<Permission,PermissionDto>().ReverseMap();
            CreateMap<EmployeeType, EmployeeTypeDto>().ReverseMap();
            CreateMap<EmployeeStatusType,EmployeeStatusTypeDto>().ReverseMap();
            CreateMap<Menu,MenuDto>().ReverseMap();
            CreateMap<OrganizationNavDto, OrganizationNavTreeViewDto>().ReverseMap();
            CreateMap<MarketAllocation, MarketAllocationDto>().ReverseMap();
            CreateMap<MarketMaster, MarketMasterDto>().ReverseMap();
            CreateMap<AllocationType, AllocationTypeDto>().ReverseMap();
            CreateMap<CogsType, CogsTypeDto>().ReverseMap();
            CreateMap<OtherType, OtherTypeDto>().ReverseMap();
            CreateMap<CompensationHistorical, CompensationHistoricalDto>().ReverseMap();
            CreateMap<JobRoleHistorical, JobRoleHistoricalDto>().ReverseMap();
            CreateMap<PayTypeHistorical, PayTypeHistoricalDto>().ReverseMap();
            CreateMap<EmployeeStatusHistorical, EmployeeStatusHistoricalDto>().ReverseMap();
            CreateMap<SalesForecast, SalesForecastDto>().ReverseMap();






        }
    }
}
