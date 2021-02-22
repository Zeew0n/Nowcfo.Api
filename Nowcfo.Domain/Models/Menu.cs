namespace Nowcfo.Domain.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Menu")]
    public class Menu
    {
        public Menu()
        {
            MenuPermissions = new HashSet<MenuPermission>();
        }

        [StringLength(100)]
        public string MenuId { get; set; }

        [Required]
        [StringLength(100)]
        public string MenuName { get; set; }

        public int MenuLevel { get; set; }

        [StringLength(100)]
        public string UnderMenuId { get; set; }

        [StringLength(100)]
        public string Icon { get; set; }

        public int? DisplayOrder { get; set; }

        public bool? IsActive { get; set; }


        public Menu MenuOne { get; set; }

        public ICollection<Menu> Menus { get; set; }

        public ICollection<MenuPermission> MenuPermissions { get; set; }
    }
}
