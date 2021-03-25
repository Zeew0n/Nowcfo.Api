using System.ComponentModel.DataAnnotations;

namespace Nowcfo.Domain.Models
{
    public class EmployeeOrgPermission: BaseEntity, ISoftDeletableEntity
    {
        [Key]
        public int EmployeeOrganizationPermissionId { get; set; }
        public int? Employee_Id { get; set; }
        public int? Organization_Id { get; set; }
    }
}
