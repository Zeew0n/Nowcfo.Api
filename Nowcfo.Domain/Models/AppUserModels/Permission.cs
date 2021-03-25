using Nowcfo.Domain.Models.AppUserModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nowcfo.Domain.Models
{
    [Table("Permission")]
    public class Permission
    {
        public Guid Id { get; private set; }

        [MaxLength(100)]
        public string Name { get; private set; }

        [MaxLength(250)]
        public string Slug { get; private set; }

        [MaxLength(100)]
        public string? Icon { get; private set; }

        public string? Group { get; private set; }
        public string? SubGroup { get; private set; }
        public ICollection<RolePermission> RolePermissions { get; set; }

        public Guid MenuId { get; set; }
        public Menu Menu { get; set; }

        //For ef
        private Permission() { }

        public Permission(
            Guid id,
            string name,
            string slug,
            string group,
            Guid menudId,
            string subGroup = ""
            )
        {
            Id = id;
            Name = name;
            Slug = slug;
            Group = group;
            SubGroup = subGroup;
            MenuId = menudId;
        }
    }
}