namespace Nowcfo.Application.DTO
{
    public class EmployeeInfoDto
    {
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }

        public int? OrganizationId { get; set; }

        public int? DesignationId { get; set; }

        public bool? IsSupervisor { get; set; }

        public int? SupervisorId { get; set; }

        public string PayType { get; set; }
        public string Pay { get; set; }
        public string OverTimeRate { get; set; }

    }
}
