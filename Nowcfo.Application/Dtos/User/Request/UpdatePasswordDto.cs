namespace Nowcfo.Application.Dtos.User.Request
{
    public class UpdatePasswordDto
    {
        public string UserName { get; set; }
        public string CurrentPassword { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
