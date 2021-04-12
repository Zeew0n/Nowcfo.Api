using System;

namespace Nowcfo.Domain.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Menu")]
    public class Menu: BaseEntity, ISoftDeletableEntity
    {

        public Menu()
        {
            Permissions = new HashSet<Permission>();
        }

        public Menu(Guid id, string name )
        {
            Id = id;
            MenuName = name;
        }

       [Key]
        public Guid Id { get; set; }

        [Required]
        [StringLength(100)]
        public string MenuName { get; set; }

        public Guid? UnderMenuId { get; set; }

        public string? NavigateUrl { get; set; }

        public int? MenuLevel { get; set; }

        public string? Icon { get; set; }

        public int? DisplayOrder { get; set; }


        public ICollection<Permission> Permissions { get; set; }
    }
}
