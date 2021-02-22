using System;
using System.ComponentModel.DataAnnotations;

namespace Nowcfo.Application.Dtos.User.Request
{
    public class UserRegisterDto
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public string FullName { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public Guid RoleId { get; set; }

        [Required]
        public string Password { get; set; }
    }
}