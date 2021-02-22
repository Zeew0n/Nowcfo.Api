using System.ComponentModel.DataAnnotations;

namespace Nowcfo.Application.DTO.User.Role
{
    public class RoleRequestDTO
    {
        [Required]
        [MaxLength(25)]
        public string RoleName { get; set; }
    }
}