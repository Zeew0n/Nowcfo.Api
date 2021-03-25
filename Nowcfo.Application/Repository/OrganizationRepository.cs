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
    public class OrganizationRepository : IOrganizationRepository
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public OrganizationRepository(IApplicationDbContext context, IMapper mapper)
        {
            _dbContext = context;
            _mapper = mapper;
        }

        public async Task<OrganizationDto> GetByIdAsync(int id)
        {
            try
            {

                var organization = await _dbContext.Organizations.AsNoTracking().SingleOrDefaultAsync(x => x.OrganizationId == id);
                return _mapper.Map<OrganizationDto>(organization);
            }
            catch (Exception ex)
            {
                Log.Error("Error: { ErrorMessage},{ ErrorDetails}", ex.Message, ex.StackTrace);
                throw;
            }
        }

        public async Task<List<OrganizationDto>> GetAllAsync()
        {
            try
            {
                var result = await (from o in _dbContext.Organizations
                                    join os in _dbContext.Organizations on o.ParentOrganizationId equals os.OrganizationId into og
                                    from os in og.DefaultIfEmpty()
                                    select new OrganizationDto
                                    {
                                        OrganizationId = o.OrganizationId,
                                        OrganizationName = o.OrganizationName,
                                        HasParent = o.HasParent,
                                        ParentOrganizationId = o.ParentOrganizationId,
                                        ParentOrganization = o.ParentOrganizationId == null ? "Head Organization" : os.OrganizationName
                                    }).ToListAsync();


                return result;
            }
            catch (Exception e)
            {
                Log.Error("Error: { ErrorMessage},{ ErrorDetails}", e.Message, e.StackTrace);
                throw;
            }
        }


        public async Task CreateAsync(Organization model)
        {
            try
            {
                await _dbContext.Organizations.AddAsync(model);
            }
            catch (Exception e)
            {
                Log.Error("Error: { ErrorMessage},{ ErrorDetails}", e.Message, e.StackTrace);
                throw;
            }
        }

        public void Update(Organization model)
        {
            try
            {
                _dbContext.Organizations.Update(model);
            }
            catch (Exception e)
            {
                Log.Error("Error: { ErrorMessage},{ ErrorDetails}", e.Message, e.StackTrace);
                throw;
            }
        }

        public void Delete(Organization model)
        {
            try
            {
                _dbContext.Organizations.Remove(model);
            }
            catch (Exception ex)
            {
                Log.Error("Error: { ErrorMessage},{ ErrorDetails}", ex.Message, ex.StackTrace);
                throw;
            }
        }


        public async Task<List<OrganizationNavTreeViewDto>> GetOrganizationTreeHierarchy()
        {
            try
            {
                var organizationList = await _dbContext.Organizations.Select(x =>
                    new OrganizationNavDto
                    {
                        Value = x.OrganizationId,
                        Text = x.OrganizationName,
                        ParentOrganizationId = x.ParentOrganizationId
                    }
                ).ToListAsync();

                var tree = GetTree(organizationList, null);

                return _mapper.Map<List<OrganizationNavTreeViewDto>>(tree);

            }
            catch (Exception e)
            {
                Log.Error("Error: { ErrorMessage},{ ErrorDetails}", e.Message, e.StackTrace);
                throw;
            }
        }

        private static List<OrganizationNavDto> GetTree(List<OrganizationNavDto> list, int? parentId)
        {
            return list.Where(x => x.ParentOrganizationId == parentId).Select(x =>
            new OrganizationNavDto
            {
                Value = x.Value,
                Text = x.Text,
                Children = GetTree(list, x.Value)

            }).ToList();

        }

        public async Task<EmployeesByOrganizationHierarchyDto> GetEmployeesByOrganizationHierarchy(int organizationId)
        {
            try
            {
                var organizationList = (await (from o in _dbContext.Organizations
                                               join e in _dbContext.EmployeeInfos
                                                   on o.OrganizationId equals e.OrganizationId into eg
                                               from c in eg.DefaultIfEmpty()
                                               join o2 in _dbContext.Organizations
                                                   on o.ParentOrganizationId equals o2.OrganizationId into og
                                               from d in og.DefaultIfEmpty()
                                               select new
                                               {
                                                   OrganizationId = o.OrganizationId,
                                                   Organization = o.OrganizationName,
                                                   ParentOrganizationId = o.ParentOrganizationId,
                                                   EmployeeId = c == null ? 0 : c.EmployeeId,
                                                   EmployeeName = c == null ? null : c.EmployeeName,
                                                   Email = c.Email,
                                                   Phone = c.Phone,
                                                   Address = c.Address,
                                                   City = c.City
                                               }
                     ).ToListAsync()).GroupBy(q => q.OrganizationId)
                     .Select(q => new EmployeesByOrganizationHierarchyDto
                     {
                         OrganizationId = q.Key,
                         Organization = q.Select(w => w.Organization).FirstOrDefault(),
                         ParentOrganizationId = q.Select(w => w.ParentOrganizationId).FirstOrDefault(),
                         Employees = _mapper.Map<List<EmployeeInfoDto>>(q.Where(w => w.EmployeeId != 0).Select(w => new EmployeeInfo
                         {
                             EmployeeId = w.EmployeeId,
                             EmployeeName = w.EmployeeName,
                             Email = w.Email,
                             Address = w.Address,
                             City = w.City
                         }).ToList())

                     }).ToList();
                var top = organizationList.FirstOrDefault(x => x.OrganizationId == organizationId);

                var tree = GetTreexx(organizationList, organizationId);
                if (top != null)
                {
                    top.ChildOrganization = tree;
                }

                return top;
            }
            catch (Exception e)
            {
                Log.Error("Error: { ErrorMessage},{ ErrorDetails}", e.Message, e.StackTrace);
                throw;
            }
        }

        private static List<EmployeesByOrganizationHierarchyDto> GetTreexx(List<EmployeesByOrganizationHierarchyDto> list, int? parentId)
        {
            return list.Where(x => x.ParentOrganizationId == parentId).Select(x =>
            {
                var a = x.ParentOrganizationId;
                return
                    new EmployeesByOrganizationHierarchyDto
                    {
                        Organization = x.Organization,
                        Employees = x.Employees,
                        OrganizationId = x.OrganizationId,
                        ChildOrganization = GetTreexx(list, x.OrganizationId)

                    };
            }).ToList();

        }


    }
}
