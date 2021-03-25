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

        public async Task<EmployeeInfoDto> GetByIdAsync(int id)
        {
            try
            {

                var employee = await _dbContext.EmployeeInfos.AsNoTracking().SingleOrDefaultAsync(x => x.EmployeeId == id);
                return _mapper.Map<EmployeeInfoDto>(employee);
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
                //var employees = await _dbContext.EmployeeInfos.ToListAsync();
                //return _mapper.Map<List<EmployeeInfoDto>>(employees);

                var result = await (from o in _dbContext.EmployeeInfos
                                    join os in _dbContext.Organizations on o.OrganizationId equals os.OrganizationId
                                    join ds in _dbContext.Designations on o.DesignationId equals ds.DesignationId 
                                    select new EmployeeInfoDto
                                    {
                                        EmployeeId = o.EmployeeId,
                                        EmployeeName = o.EmployeeName,
                                        Email = o.Email,
                                        PhoneNumber = o.Phone,
                                        Address = o.Address,
                                        City=o.City,
                                        ZipCode=o.ZipCode,
                                        DesignationName=ds.DesignationName,
                                        DesignationId=ds.DesignationId,
                                        OrganizationName=os.OrganizationName,
                                        OrganizationId=os.OrganizationId,
                                        PayType=o.PayType,
                                        Pay=o.Pay,
                                        OverTimeRate=o.OverTimeRate,
                                        IsSupervisor=o.IsSupervisor,
                                        SuperVisorId=o.SupervisorId,
                                        IsActive= o.IsActive
                                    }).Where(m=>m.IsActive==true).ToListAsync();


                return result;
            }
            catch (Exception e)
            {
                Log.Error("Error: { ErrorMessage},{ ErrorDetails}", e.Message, e.StackTrace);
                throw;
            }
        }

        public async Task<List<EmployeeInfoDto>> GetAllSuperAdmins(int orgId)
        {
            try
            {
                var employees = await _dbContext.EmployeeInfos.Where(m => m.OrganizationId == orgId && m.IsSupervisor.Value).ToListAsync();
                return _mapper.Map<List<EmployeeInfoDto>>(employees);
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
                var empOrg = new List<EmployeeOrgPermission>();
                var employee = _mapper.Map<EmployeeInfoDto,EmployeeInfo>(model);
            
                await _dbContext.EmployeeInfos.AddAsync(employee);
                 var y = _dbContext.SaveChange();
                var x = employee.EmployeeId;

               
                foreach (var employees in model.EmployeePermissions)
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

        public void Update(EmployeeInfo model)
        {
            try
            {
                _dbContext.EmployeeInfos.Update(model);
            }
            catch (Exception e)
            {
                Log.Error("Error: { ErrorMessage},{ ErrorDetails}", e.Message, e.StackTrace);
                throw;
            }
        }

        public void Delete(EmployeeInfo model)
        {
            try
            {
                model.IsActive = false;
                _dbContext.EmployeeInfos.Update(model);
            }
            catch (Exception ex)
            {
                Log.Error("Error: { ErrorMessage},{ ErrorDetails}", ex.Message, ex.StackTrace);
                throw;
            }
        }
    }
}
