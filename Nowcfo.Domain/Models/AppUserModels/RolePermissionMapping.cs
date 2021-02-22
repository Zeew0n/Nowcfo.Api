using System;
using System.ComponentModel.DataAnnotations;

namespace Nowcfo.Domain.Models.AppUserModels
{
    public class RolePermissionMapping
    {
        [Key]
        public Guid RoleId { get; set; }
        public Guid PermissionId { get; set; }
        public AppRole Role { get; set; }
        public RolePermission Permission { get; set; }
    }
}