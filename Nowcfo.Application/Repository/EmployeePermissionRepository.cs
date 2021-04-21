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
    public class EmployeePermissionRepository : IEmployeePermissionRepository
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public EmployeePermissionRepository(IApplicationDbContext context, IMapper mapper)
        {
            _dbContext = context;
            _mapper = mapper;
        }


        public async Task<List<OrganizationDto>> GetLevelOrganizations(int organizationId)
        {
            try
            {

                var organizations = await (from op in _dbContext.Organizations.Where (x => x.ParentOrganizationId == organizationId)

                                               select new OrganizationDto

                                               {
                                                   OrganizationId= op.OrganizationId,
                                                   OrganizationName=op.OrganizationName
                                               }).ToListAsync();

                return _mapper.Map<List<OrganizationDto>>(organizations);

            }
            catch (Exception e)
            {
                Log.Error("Error: { ErrorMessage},{ ErrorDetails}", e.Message, e.StackTrace);
                throw;
            }
        }



        public async Task<EmployeePermissionDto> GetByIdAsync(int id)
        {
            try
            {
                return await (from o in _dbContext.EmployeePermissions
                              join os in _dbContext.Organizations on o.LevelOne equals os.OrganizationId
                              join os1 in _dbContext.Organizations on o.LevelTwo equals os1.OrganizationId

                              join os2 in _dbContext.Organizations on o.LevelThree equals os2.OrganizationId

                              join e in _dbContext.EmployeeInfos on o.ReferenceId equals e.EmployeeId

                              join ep in _dbContext.EmployeeInfos on o.EmployeeId equals ep.EmployeeId

                              where o.PermissionId == id
                              select new EmployeePermissionDto
                              {
                                  PermissionId= o.PermissionId,
                                  EmployeeId=o.EmployeeId,
                                  LevelOne= o.LevelOne,
                                  LevelTwo= o.LevelTwo,
                                  LevelThree=o.LevelThree,
                                  ReferenceId= o.ReferenceId

                              }).FirstOrDefaultAsync(); ;

            }
            catch (Exception ex)
            {
                Log.Error("Error: { ErrorMessage},{ ErrorDetails}", ex.Message, ex.StackTrace);
                throw;
            }
        }



        public async Task<List<EmployeePermissionDto>> GetAllAsync()
        {
            try
            {
                return await (from o in _dbContext.EmployeePermissions
                              join os in _dbContext.Organizations on o.LevelOne equals os.OrganizationId
                              join os1 in _dbContext.Organizations on o.LevelTwo equals os1.OrganizationId

                              join os2 in _dbContext.Organizations on o.LevelThree equals os2.OrganizationId

                              join e in _dbContext.EmployeeInfos on o.ReferenceId equals e.EmployeeId

                              join ep in _dbContext.EmployeeInfos on o.EmployeeId equals ep.EmployeeId

                              select new EmployeePermissionDto
                              {
                                  PermissionId=o.PermissionId,
                                  EmployeeId = o.EmployeeId,
                                  EmployeeName=ep.EmployeeName,
                                  Email= ep.Email,
                                  LevelOne = o.LevelOne,
                                  OrganizationName=os.OrganizationName,
                                  LevelTwo = o.LevelTwo,
                                  RegionName=os1.OrganizationName,
                                  LevelThree = o.LevelThree,
                                  MarketName=os2.OrganizationName,
                                  ReferenceId=o.ReferenceId,
                                  ReferenceName=e.EmployeeName,
                                  ReferenceEmail=e.Email


                              }).ToListAsync();

            }
            catch (Exception e)
            {
                Log.Error("Error: { ErrorMessage},{ ErrorDetails}", e.Message, e.StackTrace);
                throw;
            }
        }





        public async Task CreateAsync(EmployeePermissionDto model)
        {
            try
            {
                var employeePermission = _mapper.Map<EmployeePermissionDto, EmployeePermission>(model);
                await _dbContext.EmployeePermissions.AddAsync(employeePermission);
                _dbContext.SaveChange();
            }
            catch (Exception e)
            {
                Log.Error("Error: { ErrorMessage},{ ErrorDetails}", e.Message, e.StackTrace);
                throw;
            }
        }


        public void Update(EmployeePermissionDto model)
        {
            try
            {
                var employee = _mapper.Map<EmployeePermissionDto, EmployeePermission>(model);
                _dbContext.EmployeePermissions.Update(employee);
                _dbContext.SaveChange();


            }
            catch (Exception e)
            {
                Log.Error("Error: { ErrorMessage},{ ErrorDetails}", e.Message, e.StackTrace);
                throw;
            }
        }


        public void Delete(EmployeePermissionDto model)
        {
            try
            {
                var employeePermission = _mapper.Map<EmployeePermissionDto, EmployeePermission>(model);
                _dbContext.EmployeePermissions.Remove(employeePermission);
            }

            catch (Exception ex)
            {
                Log.Error("Error: { ErrorMessage},{ ErrorDetails}", ex.Message, ex.StackTrace);
                throw;
            }
        }
    }
}
