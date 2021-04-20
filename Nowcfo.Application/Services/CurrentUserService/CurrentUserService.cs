using Microsoft.AspNetCore.Http;
using Nowcfo.Application.Helper;
using System;
using System.Linq;

namespace Nowcfo.Application.Services.CurrentUserService
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _context;

        public CurrentUserService(IHttpContextAccessor context)
        {
            _context = context;
        }

        public Guid GetUserId()
        {

            var userId = _context.HttpContext?.User?.Claims?.FirstOrDefault(c => c.Type == AuthConstants.JwtId)?.Value;
            return userId == null ? Guid.Empty : Guid.Parse(userId);
        }
    }
}