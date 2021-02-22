using System;

namespace Nowcfo.Application.Dtos.User.Response
{
    public class UserDetailsDto
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public Guid RoleId { get; set; }
        public string Role { get; set; }
        public string PhoneNumber { get; set; }
    }
}