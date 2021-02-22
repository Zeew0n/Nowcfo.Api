using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Nowcfo.Application.Services.CurrentUserService;

namespace Nowcfo.API.Extensions
{
    public static class ServiceExtensions
    {
        public static void AddWbApiLayer(this IServiceCollection services)
        {
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<ICurrentUserService, CurrentUserService>();
        }
    }
}