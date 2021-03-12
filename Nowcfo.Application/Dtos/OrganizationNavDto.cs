using Nowcfo.Application.DTO;
using System.Collections.Generic;
using Nowcfo.Domain.Models;

namespace Nowcfo.Application.Dtos
{
    public class OrganizationNavDto
    {
        public int Value { get; set; }
        public string Text { get; set; }
        public int? ParentOrganizationId { get; set; }
        public List<OrganizationNavDto> Children { get; set; }
    }

    public class OrganizationNavTreeViewDto
    {
        public int Value { get; set; }
        public string Text { get; set; }
        public List<OrganizationNavDto> Children { get; set; }
        public bool Collapsed => false;
    }


    public class EmployeesByOrganizationHierarchyDto
    {
        public int OrganizationId { get; set; }
        public string Organization { get; set; }
        public int? ParentOrganizationId { get; set; }
        
        public List<EmployeeInfoDto> Employees { get; set; }
        public List<EmployeesByOrganizationHierarchyDto> ChildOrganization { get; set; }
    }
}