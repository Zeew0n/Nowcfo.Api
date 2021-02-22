using Nowcfo.Application.Dtos.User.RefreshToken;
using Nowcfo.Domain.Models.AppUserModels;

namespace Nowcfo.Application.Extensions
{
    public static class RefreshTokenExtension
    {
        public static RefreshTokenResponseDto MapToRefreshTokenResponseDTO(this RefreshToken refToken)
        {
            return new RefreshTokenResponseDto
            {
                Token = refToken.Token,
                ExpiryDate = refToken.ExpiryDate,
                IsExpired = refToken.IsExpired
            };
        }
    }
}