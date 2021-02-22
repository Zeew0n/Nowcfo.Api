using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace Nowcfo.Domain.Models.AppUserModels
{
    public class AppRole : IdentityRole<Guid>, ISoftDeletableEntity
    {
        public ICollection<RolePermissionMapping> RolePermissions { get; set; }
    }
}
