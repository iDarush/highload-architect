using Architect.Web.BLL.Services;
using Architect.Web.BLL.Services.Interfaces;

namespace Architect.Web.BLL;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddBll(this IServiceCollection services)
    {
        services
            .AddScoped<IAuthService, AuthService>()
            .AddScoped<IUserService, UserService>();

        return services;
    }
}
