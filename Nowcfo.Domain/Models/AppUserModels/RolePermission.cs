using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nowcfo.Domain.Models.AppUserModels
{
    [Table("RolePermission")] 
    public class RolePermission
    {
        [Key]
        public Guid RoleId { get; set; }
        public Guid PermissionId { get; set; }
        public AppRole Role { get; set; }
        public Permission Permission { get; set; }
    }
}