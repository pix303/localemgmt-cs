using EventSourcingStore;
using Localemgmt.Application.Users.Commons;
using Localemgmt.Infrastructure.Repositories.InMemory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Localemgmt.Infrastructure;

public static class DependencyInjection
{

	const string StoreTableName = "Store:TableName";
	const string StoreConnection = "Store:Connection";

	public static IServiceCollection AddInfrastructure(this IServiceCollection services, string dbType)
	{
		services.AddSingleton<IUsersRepository, UsersRepository>();

		if (dbType == "Dynamo")
		{
			// config as singleton hosted service for async init
			services.AddHostedService<IEventStore>(serviceProvider =>
			{
				var config = serviceProvider.GetRequiredService<IConfiguration>();
				var tablename = config.GetSection(StoreTableName).Value;
				// can be null if using aws credentials
				var localhost = config.GetSection(StoreConnection).Value;
				return new DynamoDBEventStore(tablename ?? "no-table-name", localhost);
			});

			// register as service for controller and projection consumers
			services.AddSingleton<IEventStore>(serviceProvider =>
				serviceProvider.GetServices<IHostedService>()
							  .OfType<DynamoDBEventStore>()
							  .First()
							);
		}

		if (dbType == "Postgres")
		{
			// config as singleton hosted service for async init
			services.AddHostedService<IEventStore>(serviceProvider =>
			{
				var config = serviceProvider.GetRequiredService<IConfiguration>();

				var tablename = config.GetSection(StoreTableName).Value;
				if (tablename is null)
				{
					throw new Exception("no tablename string");
				}

				var connectionString = config.GetSection(StoreConnection).Value;
				if (connectionString is null)
				{
					throw new Exception("no connection string");
				}
				var store = new PostgresEventStore(connectionString, tablename);
				return store;
			});

			// register as service for controller and projection consumers
			services.AddTransient<IEventStore>(serviceProvider =>
				serviceProvider.GetServices<IHostedService>()
							  .OfType<PostgresEventStore>()
							  .First()
							);
		}

		return services;
	}
}
