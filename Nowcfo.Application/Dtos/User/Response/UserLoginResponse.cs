using System.Collections.Generic;

namespace Nowcfo.Application.Dtos.User.Response
{
    public class UserLoginResponse
    {
        public string Message { get; set; }
        public bool IsSuccess { get; set; }
        public IEnumerable<string> Errors { get; set; }
    }
}
