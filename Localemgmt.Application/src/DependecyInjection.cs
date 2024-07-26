using Localemgmt.Application.Service.Authentication;
using Microsoft.Extensions.DependencyInjection;

namespace Localemgmt.Application;

public static class DependecyInjection
{
	public static IServiceCollection AddApplication(this IServiceCollection services)
	{
		services.AddScoped<IAuthenticationService, AuthenticationService>();
		return services;
	}
}
