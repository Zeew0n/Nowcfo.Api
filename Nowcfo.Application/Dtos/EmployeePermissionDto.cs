namespace Nowcfo.Application.Dtos
{
    public class EmployeePermissionDto
    {
        public int PermissionId { get; set; }
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public int LevelOne { get; set; }
        public string OrganizationName { get; set; }
        public int LevelTwo { get; set; }
        public string RegionName { get; set; }
        public int LevelThree { get; set; }
        public string MarketName { get; set; }
        public int ReferenceId { get; set; }
        public string ReferenceName { get; set; }
        public string ReferenceEmail { get; set; }
    }
}
