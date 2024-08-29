using Localemgmt.Application.Services.Users;
using Localemgmt.Application.Services.Messages;
using Localemgmt.Application.src.Services.Messages;
using Microsoft.Extensions.DependencyInjection;

namespace Localemgmt.Application;

public static class DependecyInjection
{
	public static IServiceCollection AddApplication(this IServiceCollection services)
	{
		services.AddScoped<IUsersCommandService, UsersCommandService>();
		services.AddScoped<IUsersQueryService, UsersQueryService>();
		services.AddScoped<IGenericMessageService, GenericMessageService>();
		return services;
	}
}
