namespace Nowcfo.Domain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;


    [Table("EmployeeInfo")]
    public class EmployeeInfo: BaseEntity, ISoftDeletableEntity
    {
        public EmployeeInfo()
        {
            EmployeeInfos = new HashSet<EmployeeInfo>();
        }

        [Key] 
        public int EmployeeId { get; set; }
        [Required] 
        [StringLength(100)] 
        public string EmployeeName { get; set; }

        public string  Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public string State { get; set; }

        public int? OrganizationId { get; set; }

        public int? DesignationId { get; set; }

        public bool? IsSupervisor { get; set; }

        public int? SupervisorId { get; set; }
        public int? EmployeeTypeId { get; set; }

        public int? StatusId { get; set; }

        public string PayType { get; set; }
        public string Pay { get; set; }
        public string OverTimeRate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? TerminationDate { get; set; }
        public Designation Designation { get; set; }

        public Organization Organization { get; set; }
        public EmployeeType EmployeeType { get; set; }
        public EmployeeStatusType EmployeeStatusType { get; set; }
        public ICollection<EmployeeInfo> EmployeeInfos { get; set; }

        public EmployeeInfo Employee { get; set; }

    }
}