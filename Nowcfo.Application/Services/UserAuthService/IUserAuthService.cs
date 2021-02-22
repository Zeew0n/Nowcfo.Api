using Nowcfo.Application.Dtos.User.Request;
using Nowcfo.Application.Dtos.User.Response;
using System.Threading.Tasks;

namespace Nowcfo.Application.Services.UserAuthService
{
    public interface IUserAuthService
    {
        Task<AuthenticationResponseDto> AuthenticateAsync(AuthenticationRequestDto request);

        Task<AuthenticationResponseDto> AuthenticateTenantAsync(AuthenticationRequestDto request);

        Task<AuthenticationResponseDto> RefreshTokenAsync(string refreshToken);

        Task<bool> RevokeTokenAsync(string token);
    }
}