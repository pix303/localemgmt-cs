﻿using EventSourcingStore;
using Localemgmt.Application.Users.Commons;
using Localemgmt.Infrastructure.Repositories.InMemory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Localemgmt.Infrastructure;

public static class DependencyInjection
{
	public static IServiceCollection AddInfrastructure(this IServiceCollection services)
	{
		services.AddSingleton<IUsersRepository, UsersRepository>();

		// config as singleton for async init
		services.AddHostedService<IEventStore>(serviceProvider =>
		{
			Console.WriteLine("configuro infra");
			var config = serviceProvider.GetRequiredService<IConfiguration>();
			var tablename = config.GetSection("Store:TableName").Value;
			var localhost = config.GetSection("Store:localhost").Value;
			Console.WriteLine(localhost);
			return new DynamoDBEventStore(tablename ?? "no-table-name", localhost);
		});

		// register as service for controller and projection consumers
		services.AddSingleton<IEventStore>(serviceProvider =>
			serviceProvider.GetServices<IHostedService>()
						  .OfType<DynamoDBEventStore>()
						  .First()
						);

		return services;
	}
}
