using System;

namespace Nowcfo.Application.Services.CurrentUserService
{
    public interface ICurrentUserService
    {
        Guid GetUserId();
    }
}