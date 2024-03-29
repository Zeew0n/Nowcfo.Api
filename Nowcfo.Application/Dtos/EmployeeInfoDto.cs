﻿namespace Nowcfo.Application.Dtos
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
        public string State { get; set; }
        public int? OrganizationId { get; set; }
        public string OrganizationName { get; set; }
        public int? DesignationId { get; set; }
        public string DesignationName { get; set; }
        public bool? IsSupervisor { get; set; }
        public int? SuperVisorId { get; set; }
        public int? EmployeeTypeId { get; set; }
        public string EmployeeTypeName { get; set; }
        public int? StatusId { get; set; }
        public string StatusName { get; set; }
        public string PayType { get; set; }
        public string Pay { get; set; }
        public string OverTimeRate { get; set; }
        public string StartDate { get; set; }
        public string TerminationDate { get; set; }


    }

}
