using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Nowcfo.Application.Dtos;
using Nowcfo.Application.Helper.Pagination;
using Nowcfo.Application.IRepository;
using Nowcfo.Domain.Models;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nowcfo.Application.Repository
{
    public class EmployeeRepository : IEmployeeRepository
    {
    
        private readonly IApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public EmployeeRepository(IApplicationDbContext context, IMapper mapper)
        {
            _dbContext = context;
            _mapper = mapper;
        }

        public async Task<EmployeeInfoDto> GetByIdAsync(int id)
        {
            try
            {
                return  await (from o in _dbContext.EmployeeInfos
                                    join os in _dbContext.Organizations.IgnoreQueryFilters() on o.OrganizationId equals os.OrganizationId
                                    join ds in _dbContext.Designations on o.DesignationId equals ds.DesignationId
                                    join et in _dbContext.EmployeeTypes on o.EmployeeTypeId equals et.EmployeeTypeId
                                    join es in _dbContext.EmployeeStatusTypes on o.StatusId equals es.StatusId
                               where o.EmployeeId == id
                                    select new EmployeeInfoDto
                                    {
                                        EmployeeId = o.EmployeeId,
                                        EmployeeName = o.EmployeeName,
                                        Email = o.Email,
                                        Phone = o.Phone,
                                        Address = o.Address,
                                        City = o.City,
                                        ZipCode = o.ZipCode,
                                        State = o.State,
                                        DesignationName = ds.DesignationName,
                                        DesignationId = ds.DesignationId,
                                        OrganizationName = os.OrganizationName,
                                        OrganizationId = os.OrganizationId,
                                        PayType = o.PayType,
                                        Pay = o.Pay,
                                        OverTimeRate = o.OverTimeRate,
                                        IsSupervisor = o.IsSupervisor,
                                        SuperVisorId = o.SupervisorId,
                                        EmployeeTypeId=o.EmployeeTypeId,
                                        EmployeeTypeName=et.EmployeeTypeName,
                                        StatusId= es.StatusId,
                                        StatusName=es.StatusName
                                    }).FirstOrDefaultAsync();

            }
            catch (Exception ex)
            {
                Log.Error("Error: { ErrorMessage},{ ErrorDetails}", ex.Message, ex.StackTrace);
                throw;
            }
        }
        public async Task<List<EmployeeInfoDto>> GetAllAsync()
        {
            try
            {
                return  await (from o in _dbContext.EmployeeInfos
                                  join os in _dbContext.Organizations on o.OrganizationId equals os.OrganizationId
                                  join ds in _dbContext.Designations on o.DesignationId equals ds.DesignationId
                               join et in _dbContext.EmployeeTypes on o.EmployeeTypeId equals et.EmployeeTypeId
                               join es in _dbContext.EmployeeStatusTypes on o.StatusId equals es.StatusId
                               select new EmployeeInfoDto
                                  {
                                      EmployeeId = o.EmployeeId,
                                      EmployeeName = o.EmployeeName,
                                      Email = o.Email,
                                      Phone = o.Phone,
                                      Address = o.Address,
                                      City = o.City,
                                      ZipCode = o.ZipCode,
                                      State = o.State,
                                      DesignationName = ds.DesignationName,
                                      DesignationId = ds.DesignationId,
                                      OrganizationName = os.OrganizationName,
                                      OrganizationId = os.OrganizationId,
                                      PayType = o.PayType,
                                      Pay = o.Pay,
                                      OverTimeRate = o.OverTimeRate,
                                      IsSupervisor = o.IsSupervisor,
                                      SuperVisorId = o.SupervisorId,
                                      EmployeeTypeId = o.EmployeeTypeId,
                                      EmployeeTypeName = et.EmployeeTypeName,
                                      StatusId = o.StatusId,
                                      StatusName = es.StatusName
                                  }).Where(e=>e.StatusId!=3).ToListAsync();

            }
            catch (Exception e)
            {
                Log.Error("Error: { ErrorMessage},{ ErrorDetails}", e.Message, e.StackTrace);
                throw;
            }
        }




        public async Task<PagedList<EmployeeInfoDto>> GetPagedListAsync(Param param)
        {
            try
            {


             if(param.SearchType == "null" || param.SearchValue == "null")
             {

                    if(param.SearchOrg != "null" && param.SearchStatus != "null")
                    {


                        var result = (
                                      from o in _dbContext.Organizations.IgnoreQueryFilters()
                                      join e in _dbContext.EmployeeInfos on o.OrganizationId equals e.OrganizationId into eg
                                      from e in eg.DefaultIfEmpty()
                                      join ds in _dbContext.Designations on e.DesignationId equals ds.DesignationId
                                      join et in _dbContext.EmployeeTypes on e.EmployeeTypeId equals et.EmployeeTypeId
                                      join es in _dbContext.EmployeeStatusTypes on e.StatusId equals es.StatusId

                                      select new EmployeeInfoDto
                                      {
                                          EmployeeId = e.EmployeeId,
                                          EmployeeName = e.EmployeeName,
                                          Email = e.Email,
                                          Phone = e.Phone,
                                          Address = e.Address,
                                          City = e.City,
                                          ZipCode = e.ZipCode,
                                          State = e.State,
                                          PayType = e.PayType,
                                          Pay = e.Pay,
                                          OverTimeRate = e.OverTimeRate,
                                          IsSupervisor = e.IsSupervisor,
                                          SuperVisorId = e.SupervisorId,
                                          DesignationName = ds.DesignationName,
                                          DesignationId = ds.DesignationId,
                                          OrganizationName = o.OrganizationName,
                                          OrganizationId = o.OrganizationId,
                                          EmployeeTypeId = e.EmployeeTypeId,
                                          EmployeeTypeName = et.EmployeeTypeName,
                                          StatusId = e.StatusId,
                                          StatusName = es.StatusName
                                      }).Where(e => e.StatusId != 3).Where(e => e.OrganizationId.ToString() == param.SearchOrg && e.StatusId.ToString() == param.SearchStatus);



                        return await PagedList<EmployeeInfoDto>.CreateAsync(result, param.PageNumber, param.PageSize);

                    }
                    else if (param.SearchOrg != "null" && param.SearchStatus == "null")
                    {


                        var result = (
                                      from o in _dbContext.Organizations.IgnoreQueryFilters()
                                      join e in _dbContext.EmployeeInfos on o.OrganizationId equals e.OrganizationId into eg
                                      from e in eg.DefaultIfEmpty()
                                      join ds in _dbContext.Designations on e.DesignationId equals ds.DesignationId
                                      join et in _dbContext.EmployeeTypes on e.EmployeeTypeId equals et.EmployeeTypeId
                                      join es in _dbContext.EmployeeStatusTypes on e.StatusId equals es.StatusId

                                      select new EmployeeInfoDto
                                      {
                                          EmployeeId = e.EmployeeId,
                                          EmployeeName = e.EmployeeName,
                                          Email = e.Email,
                                          Phone = e.Phone,
                                          Address = e.Address,
                                          City = e.City,
                                          ZipCode = e.ZipCode,
                                          State = e.State,
                                          PayType = e.PayType,
                                          Pay = e.Pay,
                                          OverTimeRate = e.OverTimeRate,
                                          IsSupervisor = e.IsSupervisor,
                                          SuperVisorId = e.SupervisorId,
                                          DesignationName = ds.DesignationName,
                                          DesignationId = ds.DesignationId,
                                          OrganizationName = o.OrganizationName,
                                          OrganizationId = o.OrganizationId,
                                          EmployeeTypeId = e.EmployeeTypeId,
                                          EmployeeTypeName = et.EmployeeTypeName,
                                          StatusId = e.StatusId,
                                          StatusName = es.StatusName
                                      }).Where(e => e.StatusId != 3).Where(e => e.OrganizationId.ToString() == param.SearchOrg);



                        return await PagedList<EmployeeInfoDto>.CreateAsync(result, param.PageNumber, param.PageSize);




                    }
                    else
                    {



                        var result = (
                                      from o in _dbContext.Organizations.IgnoreQueryFilters()
                                      join e in _dbContext.EmployeeInfos on o.OrganizationId equals e.OrganizationId into eg
                                      from e in eg.DefaultIfEmpty()
                                      join ds in _dbContext.Designations on e.DesignationId equals ds.DesignationId
                                      join et in _dbContext.EmployeeTypes on e.EmployeeTypeId equals et.EmployeeTypeId
                                      join es in _dbContext.EmployeeStatusTypes on e.StatusId equals es.StatusId

                                      select new EmployeeInfoDto
                                      {
                                          EmployeeId = e.EmployeeId,
                                          EmployeeName = e.EmployeeName,
                                          Email = e.Email,
                                          Phone = e.Phone,
                                          Address = e.Address,
                                          City = e.City,
                                          ZipCode = e.ZipCode,
                                          State = e.State,
                                          PayType = e.PayType,
                                          Pay = e.Pay,
                                          OverTimeRate = e.OverTimeRate,
                                          IsSupervisor = e.IsSupervisor,
                                          SuperVisorId = e.SupervisorId,
                                          DesignationName = ds.DesignationName,
                                          DesignationId = ds.DesignationId,
                                          OrganizationName = o.OrganizationName,
                                          OrganizationId = o.OrganizationId,
                                          EmployeeTypeId = e.EmployeeTypeId,
                                          EmployeeTypeName = et.EmployeeTypeName,
                                          StatusId = e.StatusId,
                                          StatusName = es.StatusName
                                      }).Where(e => e.StatusId != 3);



                        return await PagedList<EmployeeInfoDto>.CreateAsync(result, param.PageNumber, param.PageSize);
                    }

                }
                else
                {
                    if (param.SearchType == "1")
                    {

                        var result = (from o in _dbContext.EmployeeInfos
                                      join os in _dbContext.Organizations on o.OrganizationId equals os.OrganizationId
                                      join ds in _dbContext.Designations on o.DesignationId equals ds.DesignationId
                                      join et in _dbContext.EmployeeTypes on o.EmployeeTypeId equals et.EmployeeTypeId
                                      join es in _dbContext.EmployeeStatusTypes on o.StatusId equals es.StatusId
                                      select new EmployeeInfoDto
                                      {
                                          EmployeeId = o.EmployeeId,
                                          EmployeeName = o.EmployeeName,
                                          Email = o.Email,
                                          Phone = o.Phone,
                                          Address = o.Address,
                                          City = o.City,
                                          ZipCode = o.ZipCode,
                                          State = o.State,
                                          DesignationName = ds.DesignationName,
                                          DesignationId = ds.DesignationId,
                                          OrganizationName = os.OrganizationName,
                                          OrganizationId = os.OrganizationId,
                                          PayType = o.PayType,
                                          Pay = o.Pay,
                                          OverTimeRate = o.OverTimeRate,
                                          IsSupervisor = o.IsSupervisor,
                                          SuperVisorId = o.SupervisorId,
                                          EmployeeTypeId = o.EmployeeTypeId,
                                          EmployeeTypeName = et.EmployeeTypeName,
                                          StatusId = o.StatusId,
                                          StatusName = es.StatusName
                                      }).Where(m=>m.EmployeeName.Contains(param.SearchValue) && m.StatusId!=3 ).Where(m=>m.OrganizationId.ToString()==param.SearchOrg).Where(m => m.StatusId.ToString() == param.SearchStatus);

                        return await PagedList<EmployeeInfoDto>.CreateAsync(result, param.PageNumber, param.PageSize);



                    }
                    else
                    {

                        var result = (from o in _dbContext.EmployeeInfos
                                      join os in _dbContext.Organizations on o.OrganizationId equals os.OrganizationId
                                      join ds in _dbContext.Designations on o.DesignationId equals ds.DesignationId
                                      join et in _dbContext.EmployeeTypes on o.EmployeeTypeId equals et.EmployeeTypeId
                                      join es in _dbContext.EmployeeStatusTypes on o.StatusId equals es.StatusId
                                      select new EmployeeInfoDto
                                      {
                                          EmployeeId = o.EmployeeId,
                                          EmployeeName = o.EmployeeName,
                                          Email = o.Email,
                                          Phone = o.Phone,
                                          Address = o.Address,
                                          City = o.City,
                                          ZipCode = o.ZipCode,
                                          State = o.State,
                                          DesignationName = ds.DesignationName,
                                          DesignationId = ds.DesignationId,
                                          OrganizationName = os.OrganizationName,
                                          OrganizationId = os.OrganizationId,
                                          PayType = o.PayType,
                                          Pay = o.Pay,
                                          OverTimeRate = o.OverTimeRate,
                                          IsSupervisor = o.IsSupervisor,
                                          SuperVisorId = o.SupervisorId,
                                          EmployeeTypeId = o.EmployeeTypeId,
                                          EmployeeTypeName = et.EmployeeTypeName,
                                          StatusId = o.StatusId,
                                          StatusName = es.StatusName
                                      }).Where(m => m.Email.Contains(param.SearchValue) && m.StatusId != 3).Where(m => m.OrganizationId.ToString() == param.SearchOrg).Where(m => m.StatusId.ToString() == param.SearchStatus);

                        return await PagedList<EmployeeInfoDto>.CreateAsync(result, param.PageNumber, param.PageSize);

                    }

                }
            }
            catch (Exception e)
            {
                Log.Error("Error: { ErrorMessage},{ ErrorDetails}", e.Message, e.StackTrace);
                throw;
            }
        }

        public async Task<List<EmployeeInfoDto>> GetAllSuperVisors()
        {
            try
            {
                var employees = await _dbContext.EmployeeInfos.Where(m => m.IsSupervisor.Value==true).ToListAsync();
                return _mapper.Map<List<EmployeeInfoDto>>(employees);
            }
            catch (Exception e)
            {
                Log.Error("Error: { ErrorMessage},{ ErrorDetails}", e.Message, e.StackTrace);
                throw;
            }
        }


        public async Task<List<EmployeeStatusTypeDto>> GetAllEmployeeStatus()
        {
            try
            {
                var status = await _dbContext.EmployeeStatusTypes.ToListAsync();
                return _mapper.Map<List<EmployeeStatusTypeDto>>(status);
            }
            catch (Exception e)
            {
                Log.Error("Error: { ErrorMessage},{ ErrorDetails}", e.Message, e.StackTrace);
                throw;
            }
        }


        public async Task<List<EmployeeTypeDto>> GetAllEmployeeTypes()
        {
            try
            {
                var employeetypes = await _dbContext.EmployeeTypes.ToListAsync();
                return _mapper.Map<List<EmployeeTypeDto>>(employeetypes);
            }
            catch (Exception e)
            {
                Log.Error("Error: { ErrorMessage},{ ErrorDetails}", e.Message, e.StackTrace);
                throw;
            }
        }


        public async Task CreateAsync(EmployeeInfoDto model)
        {
            try
            {
                var employee = _mapper.Map<EmployeeInfoDto, EmployeeInfo>(model);
                await _dbContext.EmployeeInfos.AddAsync(employee);
                _dbContext.SaveChange();
            }
            catch (Exception e)
            {
                Log.Error("Error: { ErrorMessage},{ ErrorDetails}", e.Message, e.StackTrace);
                throw;
            }
        }

        public void Update(EmployeeInfoDto model)
        {
            try
            {
                var employee = _mapper.Map<EmployeeInfoDto, EmployeeInfo>(model);
                _dbContext.EmployeeInfos.Update(employee);
                _dbContext.SaveChange();
                

            }
            catch (Exception e)
            {
                Log.Error("Error: { ErrorMessage},{ ErrorDetails}", e.Message, e.StackTrace);
                throw;
            }
        }


        public void Delete(EmployeeInfoDto model)
        {
            try
            {
                var employee = _mapper.Map<EmployeeInfoDto, EmployeeInfo>(model);
                _dbContext.EmployeeInfos.Update(employee);
            }

            catch (Exception ex)
            {
                Log.Error("Error: { ErrorMessage},{ ErrorDetails}", ex.Message, ex.StackTrace);
                throw;
            }
        }

        public async Task<List<EmployeeInfoDto>> GetEmployeesAutocompleteAsync(string searchText)
        {
            try
            {
                if (string.IsNullOrEmpty(searchText))
                {
                    return _mapper.Map<List<EmployeeInfoDto>>( await _dbContext.EmployeeInfos.Select(x => new EmployeeInfo{ EmployeeId=x.EmployeeId, EmployeeName=x.EmployeeName}).ToListAsync());
                }
                return _mapper.Map<List<EmployeeInfoDto>>( await _dbContext.EmployeeInfos.Where(x=>x.EmployeeName.ToLower().Contains(searchText.ToLower())).Select(x => new EmployeeInfo{ EmployeeId=x.EmployeeId, EmployeeName=x.EmployeeName}).ToListAsync());
            }
            catch (Exception ex)
            {
                Log.Error("Error: { ErrorMessage},{ ErrorDetails}", ex.Message, ex.StackTrace);
                throw;
            }
        }


        ////
        //public async Task<List<SyncfusionListDto>> GetSyncFusionOrganizations()
        //{
        //    try
        //    {

        //        var organizationList = await (from os in _dbContext.Organizations


        //                                      select new SyncfusionListDto
        //                                      {
        //                                          id = os.OrganizationId,
        //                                          pid = os.ParentOrganizationId,
        //                                          name = os.OrganizationName,
        //                                          hasChild = _dbContext.Organizations.Count(x => x.ParentOrganizationId == os.OrganizationId) > 0,
        //                                          expanded = os.ParentOrganizationId == null,
        //                                          isChecked = false,
        //                                      }).ToListAsync();


        //        return _mapper.Map<List<SyncfusionListDto>>(organizationList);

        //    }
        //    catch (Exception e)
        //    {
        //        Log.Error("Error: { ErrorMessage},{ ErrorDetails}", e.Message, e.StackTrace);
        //        throw;
        //    }
        //}

        //public async Task<List<UserPermissionDto>> GetCheckedPermissions(int employeeId)
        //{
        //    try
        //    {

        //        var emppermissionList = await (from op in _dbContext.EmployeeOrgPermissions.Where(x => x.Employee_Id == employeeId)

        //                                       select new UserPermissionDto

        //                                       {
        //                                           OrgId = op.Organization_Id,
        //                                       }).ToListAsync();

        //        return _mapper.Map<List<UserPermissionDto>>(emppermissionList);

        //    }
        //    catch (Exception e)
        //    {
        //        Log.Error("Error: { ErrorMessage},{ ErrorDetails}", e.Message, e.StackTrace);
        //        throw;
        //    }
        //}

        //public async Task<List<SyncfusionListDto>> GetEmployeePermissionHierarchy(int employeeId)
        //{
        //    try
        //    {

        //        var permissionList = await (from os in _dbContext.Organizations
        //                                    join o in _dbContext.EmployeeOrgPermissions.Where(x => x.Employee_Id == employeeId) on os.OrganizationId equals o.Organization_Id into p
        //                                    from q in p.DefaultIfEmpty()

        //                                    select new SyncfusionListDto
        //                                    {
        //                                        id = q != null ? (int)q.Organization_Id : os.OrganizationId,
        //                                        name = os.OrganizationName,
        //                                        isChecked = q != null,
        //                                        hasChild = _dbContext.Organizations.Count(x => x.ParentOrganizationId == os.OrganizationId) > 0,
        //                                        expanded = os.ParentOrganizationId == null,
        //                                        pid = os.ParentOrganizationId
        //                                    }).ToListAsync();

        //        return _mapper.Map<List<SyncfusionListDto>>(permissionList);

        //    }
        //    catch (Exception e)
        //    {
        //        Log.Error("Error: { ErrorMessage},{ ErrorDetails}", e.Message, e.StackTrace);
        //        throw;
        //    }
        //}
    }


}
