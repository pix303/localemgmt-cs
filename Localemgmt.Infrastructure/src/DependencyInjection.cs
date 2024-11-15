using EventSourcingStore;
using Localemgmt.Application.Users.Commons;
using Localemgmt.Infrastructure.Repositories.InMemory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Localemgmt.Infrastructure.Services;

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
				return new PostgresEventStore(dbConnector, storeSettings.TableName);
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

		// projection db
		services.AddTransient<IRetriveService>(serviceProvider =>
		{
			IDBConnectionFactory dbConnector = new NpgsqlDBConnectionFactory(storeSettings); ;
			return new PostgresProjectionService(dbConnector);
		});

		return services;
	}
}
