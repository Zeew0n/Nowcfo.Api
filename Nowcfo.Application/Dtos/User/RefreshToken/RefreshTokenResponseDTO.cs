using System;

namespace Nowcfo.Application.Dtos.User.RefreshToken
{
    public class RefreshTokenResponseDto
    {
        public string Token { get; set; }
        public DateTime ExpiryDate { get; set; }
        public bool IsExpired { get; set; }
    }
}