using System.Collections.Generic;

namespace Nowcfo.Application.Dtos
{
    public class EmployeeOrganizationPermissionNavDto
    {
        public int Value { get; set; }
        public string Text { get; set;}
        public bool Checked { get; set;}
        public int? ParentOrganizationId { get; set; }
        public List<EmployeeOrganizationPermissionNavDto> Children { get; set; }
    }
}
