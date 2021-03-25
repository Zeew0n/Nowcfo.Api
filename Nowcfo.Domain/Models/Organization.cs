namespace Nowcfo.Domain.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;


    [Table("Organization")]
    public class Organization: BaseEntity, ISoftDeletableEntity
    {
        public Organization()
        {
            EmployeesInfo = new HashSet<EmployeeInfo>();
            Organizations= new HashSet<Organization>();
        }

        [Key]
        public int OrganizationId { get; set; }

        public string OrganizationName { get; set; }

        public bool? HasParent { get; set; }

        [StringLength(100)]
        public int? ParentOrganizationId { get; set; }

        public ICollection<EmployeeInfo> EmployeesInfo { get; set; }

        public ICollection<Organization> Organizations { get; set; }

        public Organization OneOrganization { get; set; }
    }
}
