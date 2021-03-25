using System.ComponentModel.DataAnnotations;

namespace Nowcfo.Application.Dtos.User.Role
{
    public class RoleRequestDto
    {
        [Required]
        [MaxLength(25)]
        public string RoleName { get; set; }
    }
}