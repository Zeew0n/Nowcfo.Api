using System;
using System.Collections.Generic;

namespace Nowcfo.Application.Dtos
{
    public class RolePermissionDto
    {
        public RolePermissionDto()
        {
            MenuIds = new List<Guid>();
        }
        public Guid RoleId { get; set; }
        public string RoleName { get; set; }
        public List<Guid> PermissionIds { get; set; }
        public List<Guid> MenuIds { get; set; }
    
    }
}
