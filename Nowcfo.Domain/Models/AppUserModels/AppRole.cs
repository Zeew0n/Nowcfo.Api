using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace Nowcfo.Domain.Models.AppUserModels
{
    public class AppRole : IdentityRole<Guid> ,ISoftDeletableEntity
    {
        public AppRole()
        {
            RolePermissions = new HashSet<RolePermission>();
        }
        public ICollection<RolePermission> RolePermissions { get; set; }
    }
}
