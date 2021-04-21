using System.ComponentModel.DataAnnotations;

namespace Nowcfo.Domain.Models
{
    public class EmployeePermission:BaseEntity, ISoftDeletableEntity
    {
        [Key]
        public int PermissionId { get; set; }
        public int EmployeeId { get; set; }
        public int LevelOne { get; set; }
        public int LevelTwo { get; set; }
        public int LevelThree { get; set; }
        public int ReferenceId { get; set; }
    } 
}
