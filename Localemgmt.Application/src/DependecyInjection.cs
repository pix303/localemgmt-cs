using Localemgmt.Api.Config;
using Localemgmt.Application.Services.Messages;
using Localemgmt.Application.src.Services.Messages;
using Microsoft.Extensions.DependencyInjection;

namespace Localemgmt.Application;

public static class DependecyInjection
{
	public static IServiceCollection AddApplication(this IServiceCollection services)
	{
		services.AddScoped<IGenericMessageService, GenericMessageService>();
		services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(DependecyInjection).Assembly));

		MapsterConfig.RegisterMapsterConfiguration();


		return services;
	}
}
