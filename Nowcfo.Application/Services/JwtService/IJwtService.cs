using Nowcfo.Application.Dtos.User.Response;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Nowcfo.Application.Services.JwtService
{
    public interface IJwtService
    {
        ClaimsIdentity GenerateClaimsIdentity(ClaimDto claimDTO);
        Task<AuthenticationResponseDto> GenerateJwt(AppUserDto userDetails);
    }
}