using Localemgmt.Application.Users.Commons;
using Localemgmt.Infrastructure.Repositories.InMemory;
using Microsoft.Extensions.DependencyInjection;

namespace Localemgmt.Infrastructure;

public static class DependencyInjection
{
	public static IServiceCollection AddInfrastructure(this IServiceCollection services)
	{
		services.AddSingleton<IUsersRepository, UsersRepository>();
		return services;
	}
}
