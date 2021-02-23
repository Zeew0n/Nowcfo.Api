using System;

namespace Nowcfo.Application.Dtos.User.Response
{
    public class UserListDto
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string RoleId { get; set; }
        public string Role { get; set; }
        public string Status { get; set; }
    }
}