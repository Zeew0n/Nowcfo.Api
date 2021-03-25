namespace Nowcfo.API.Models
{
    public class User
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public bool IsAdmin { get; set; }
        public bool EmailConfirmed { get; set; }
    }
}