using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Nowcfo.Application.Dtos;
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

        public async Task<EmployeeInfoListDto> GetByIdAsync(int id)
        {
            try
            {

                //var query = await (from ep in _dbContext.EmployeeOrgPermissions
                //                   join o in _dbContext.Organizations on ep.Organization_Id equals o.OrganizationId
                //                    join e in _dbContext.EmployeeInfos on ep.Employee_Id equals e.EmployeeId
                //                    where e.IsActive && e.EmployeeId==id
                //                    select new EmployeeOrgPermissionListDto
                //                    {


                //                            OrganizationId = ep.Organization_Id,
                //                            OrganizationName = o.OrganizationName

                //                    }).ToListAsync();


                var result = await (from o in _dbContext.EmployeeInfos
                                    join os in _dbContext.Organizations on o.OrganizationId equals os.OrganizationId
                                    join ds in _dbContext.Designations on o.DesignationId equals ds.DesignationId
                                    where o.IsActive && o.EmployeeId == id
                                    select new EmployeeInfoListDto
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
                                        IsActive = o.IsActive,
                                        //employeepermissions =query
                                    }).FirstOrDefaultAsync();


                return result;


            }
            catch (Exception ex)
            {
                Log.Error("Error: { ErrorMessage},{ ErrorDetails}", ex.Message, ex.StackTrace);
                throw;
            }
        }

        public async Task<List<EmployeeInfoListDto>> GetAllAsync()
        {
            try
            {
                var result = await (from o in _dbContext.EmployeeInfos
                                    join os in _dbContext.Organizations on o.OrganizationId equals os.OrganizationId
                                    join ds in _dbContext.Designations on o.DesignationId equals ds.DesignationId
                                    where o.IsActive
                                    select new EmployeeInfoListDto
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
                                        IsActive = o.IsActive
                                    }).ToListAsync();

                return result;
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
                var employees = await _dbContext.EmployeeInfos.Where(m => m.IsActive && m.IsSupervisor.Value).ToListAsync();
                return _mapper.Map<List<EmployeeInfoDto>>(employees);
            }
            catch (Exception e)
            {
                Log.Error("Error: { ErrorMessage},{ ErrorDetails}", e.Message, e.StackTrace);
                throw;
            }
        }

        public async Task CreateAsync(EmployeeUpdateDto model)
        {
            try
            {
                var empOrg = new List<EmployeeOrgPermission>();
                var employee = _mapper.Map<EmployeeUpdateDto, EmployeeInfo>(model);
                employee.IsActive = true;

                await _dbContext.EmployeeInfos.AddAsync(employee);
                var y = _dbContext.SaveChange();
                var x = employee.EmployeeId;


                foreach (var employees in model.employeepermissions)
                {
                    var model1 = new EmployeeOrgPermission
                    {
                        Employee_Id = x,
                        Organization_Id = employees
                    };
                    empOrg.Add(model1);
                }
                _dbContext.EmployeeOrgPermissions.AddRange(empOrg);
                _dbContext.SaveChange();
            }
            catch (Exception e)
            {
                Log.Error("Error: { ErrorMessage},{ ErrorDetails}", e.Message, e.StackTrace);
                throw;
            }
        }

        public void Update(EmployeeUpdateDto model)
        {
            try
            {
                var empOrg = new List<EmployeeOrgPermission>();
                var employee = _mapper.Map<EmployeeUpdateDto, EmployeeInfo>(model);
                employee.IsActive = true;

                _dbContext.EmployeeInfos.Update(employee);
                var y = _dbContext.SaveChange();
                var x = employee.EmployeeId;


                var permissions = _dbContext.EmployeeOrgPermissions.Where(z => z.Employee_Id == x).ToListAsync();
                _dbContext.EmployeeOrgPermissions.RemoveRange(permissions.Result);
                _dbContext.SaveChange();


                foreach (var employees in model.employeepermissions)
                {
                    var model1 = new EmployeeOrgPermission
                    {
                        Employee_Id = x,
                        Organization_Id = employees
                    };
                    empOrg.Add(model1);
                }
                _dbContext.EmployeeOrgPermissions.UpdateRange(empOrg);
                _dbContext.SaveChange();
            }
            catch (Exception e)
            {
                Log.Error("Error: { ErrorMessage},{ ErrorDetails}", e.Message, e.StackTrace);
                throw;
            }
        }




        public async Task<List<EmployeeOrganizationPermissionNavDto>> GetEmployeePermissionHierarchy(int employeeId)
        {
            try
            {

                var permissionList = await (from os in _dbContext.Organizations
                                            join o in _dbContext.EmployeeOrgPermissions.Where(x => x.Employee_Id == employeeId) on os.OrganizationId equals o.Organization_Id into p
                                            from q in p.DefaultIfEmpty()

                                            select new EmployeeOrganizationPermissionNavDto
                                            {
                                                Value = q != null ? (int)q.Organization_Id : os.OrganizationId,
                                                Text = os.OrganizationName,
                                                Checked = q != null ? true : false,
                                                ParentOrganizationId = os.ParentOrganizationId
                                            }).ToListAsync();


                var tree = GetTree(permissionList, null);

                return _mapper.Map<List<EmployeeOrganizationPermissionNavDto>>(tree);

            }
            catch (Exception e)
            {
                Log.Error("Error: { ErrorMessage},{ ErrorDetails}", e.Message, e.StackTrace);
                throw;
            }
        }


        //For KendoUI Treeview

        public async Task<List<KendoDto>> GetKendoTreeHierarchy()
        {
            try
            {
                var organizationList = await _dbContext.Organizations.Select(x =>
                    new KendoDto
                    {
                        Id = x.OrganizationId,
                        Text = x.OrganizationName,
                        ParentOrganizationId = x.ParentOrganizationId,
                    }
                ).ToListAsync();

                var tree = GetKendoTree(organizationList, null);

                return _mapper.Map<List<KendoDto>>(tree);

            }
            catch (Exception e)
            {
                Log.Error("Error: { ErrorMessage},{ ErrorDetails}", e.Message, e.StackTrace);
                throw;
            }
        }


        private List<KendoDto> GetKendoTree(List<KendoDto> list, int? parentId)
        {
            return list.Where(x => x.ParentOrganizationId == parentId).Select(x =>
            new KendoDto
            {
                Id = x.Id,
                Text = x.Text,
                CheckType = 0,
                ChildrenCount = GetChildren(x.Id),
                //ParentOrganizationId=x.ParentOrganizationId,
                items = GetKendoTree(list, x.Id)

            }).ToList();

        }

        public int GetChildren(int? parentId)
        {
            var count = (from m in _dbContext.Organizations.ToList() select m).Count(x => x.ParentOrganizationId == parentId);
            return count;

        }

        private static List<EmployeeOrganizationPermissionNavDto> GetTree(List<EmployeeOrganizationPermissionNavDto> list, int? parentId)
        {
            return list.Where(x => x.ParentOrganizationId == parentId).Select(x =>
            new EmployeeOrganizationPermissionNavDto
            {
                Value = x.Value,
                Text = x.Text,
                Checked = x.Checked,
                Children = GetTree(list, x.Value),

            }).ToList();

        }


        public void Delete(EmployeeInfoListDto model)
        {
            try
            {
                var employee = _mapper.Map<EmployeeInfoListDto, EmployeeInfo>(model);


                employee.IsActive = false;
                _dbContext.EmployeeInfos.Update(employee);
            }

            catch (Exception ex)
            {
                Log.Error("Error: { ErrorMessage},{ ErrorDetails}", ex.Message, ex.StackTrace);
                throw;
            }
        }
    }
}
