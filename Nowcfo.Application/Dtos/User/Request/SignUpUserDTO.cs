using System;

namespace Nowcfo.Application.Dtos.User.Request
{
    public class SignUpUserDto
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}