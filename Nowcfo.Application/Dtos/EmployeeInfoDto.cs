namespace Nowcfo.Application.DTO
{
    public class EmployeeInfoDto
    {
        public string EmployeeName { get; set; }

        public int? OrganizationId { get; set; }

        public int? DesignationId { get; set; }

        public bool? IsSupervisor { get; set; }

        public int? SupervisorId { get; set; }
    }
}
