namespace Nowcfo.Domain.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("MenuPermission")]
    public  class MenuPermission
    {
        [Key]
        [StringLength(100)]
        public string MenuPermissionId { get; set; }

        public int? RoleId { get; set; }

        [Required]
        [StringLength(100)]
        public string MenuId { get; set; }

        public Menu Menu { get; set; }
    }
}
