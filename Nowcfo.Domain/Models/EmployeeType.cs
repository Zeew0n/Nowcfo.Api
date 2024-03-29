﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nowcfo.Domain.Models
{
    [Table("EmployeeType")]
    public class EmployeeType
    {
    
        [Key]
        public int EmployeeTypeId { get; set; }
        public string EmployeeTypeName { get; set; }
        public ICollection<EmployeeInfo> Employees { get; set; }
    }
}
