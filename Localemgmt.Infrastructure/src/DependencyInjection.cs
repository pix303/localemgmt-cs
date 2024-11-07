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

	public static IServiceCollection AddInfrastructure(this IServiceCollection services, ConfigurationManager configurationManager)
	{

		StoreSettings storeSettings = new();
		configurationManager.GetRequiredSection("Store").Bind(storeSettings);

		services.AddSingleton<IUsersRepository, UsersRepository>();

		if (storeSettings.DBType == "Dynamo")
		{
			// config as singleton hosted service for async init
			services.AddHostedService<IEventStore>(serviceProvider =>
			{
				var config = serviceProvider.GetRequiredService<IConfiguration>();
				var tablename = config.GetSection(StoreTableName).Value;
				// can be null if using aws credentials
				var localhost = config.GetSection(StoreConnection).Value;
				return new DynamoDBEventStore(storeSettings);
			});

			// register as service for controller and projection consumers
			services.AddSingleton<IEventStore>(serviceProvider =>
				serviceProvider.GetServices<IHostedService>()
							  .OfType<DynamoDBEventStore>()
							  .First()
							);
		}

		if (storeSettings.DBType == "Postgres")
		{
			// config as singleton hosted service for async init
			services.AddHostedService<IEventStore>(serviceProvider =>
			{
				IDBConnectionFactory dbConnector = new NpgsqlDBConnectionFactory(storeSettings); ;
				var store = new PostgresEventStore(dbConnector, storeSettings.TableName);
				return store;
			});

			// register as service for controller and projection consumers
			services.AddTransient<IEventStore>(serviceProvider =>
				serviceProvider.GetServices<IHostedService>()
							  .OfType<PostgresEventStore>()
							  .First()
							);
		}

		services.AddScoped<IDBConnectionFactory>(serviceProvider =>
		{
			IDBConnectionFactory dbConnector = new NpgsqlDBConnectionFactory(storeSettings);
			return dbConnector;
		});

		return services;
	}
}
