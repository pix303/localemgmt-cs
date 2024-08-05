using Localemgmt.Application.Service.Authentication;
using Microsoft.Extensions.DependencyInjection;

namespace Localemgmt.Infrastructure;

public static class DependencyInjection
{
	public static IServiceCollection AddInfrastructure(this IServiceCollection services)
	{
		services.AddScoped<IAuthenticationService, AuthenticationService>();
		return services;
	}
}
