using EventSourcingStore;
using Localemgmt.Application.Users.Commons;
using Localemgmt.Infrastructure.Repositories.InMemory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;


namespace Localemgmt.Infrastructure;

public static class DependencyInjection
{
	public static IServiceCollection AddInfrastructure(this IServiceCollection services)
	{
		services.AddSingleton<IUsersRepository, UsersRepository>();
		services.AddScoped<IEventStore>(serviceProvider =>
		{
			var config = serviceProvider.GetRequiredService<IConfiguration>();
			var tablename = config.GetSection("Store:TableName").Value;
			return new DynamoDBEventStore(tablename ?? "no-table-name");
		});
		return services;
	}
}
