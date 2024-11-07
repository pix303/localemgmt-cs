using EventSourcingStore;
using Localemgmt.Domain.LocaleItems.Events;
using Localemgmt.Domain.LocaleItems.Projections;
using MassTransit;
using Dapper;

namespace Localemgmt.Api.Consumer;

public record LocaleItemProjectionMessage
(
	string AggregateId
);

public record LocaleItemUpdatedMessage
(
	string AggregateId,
	string Type
);

public record LocaleItemListItem
(
	string AggregateId,
	string Lang,
	string Context,
	string Content,
	DateTime UpdatedAt,
	string UpdatedBy
);

public class LocaleItemProjectionConsumerDefinition : ConsumerDefinition<LocaleItemProjectionConsumer>
{
	public LocaleItemProjectionConsumerDefinition()
	{
		this.ConcurrentMessageLimit = 15;
	}

	protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<LocaleItemProjectionConsumer> consumerConfigurator, IRegistrationContext context)
	{
		base.ConfigureConsumer(endpointConfigurator, consumerConfigurator, context);

		endpointConfigurator.Batch<LocaleItemProjectionMessage>(config =>
		{
			config.MessageLimit = 15;
			config.TimeLimit = TimeSpan.FromSeconds(10);
			config.TimeLimitStart = BatchTimeLimitStart.FromLast;
		});
	}
}


public class LocaleItemProjectionConsumer : IConsumer<Batch<LocaleItemProjectionMessage>>
{
	readonly ILogger<LocaleItemProjectionConsumer> _logger;
	readonly IEventStore _store;
	readonly IDBConnectionFactory _dbConnection;

	public LocaleItemProjectionConsumer(ILogger<LocaleItemProjectionConsumer> logger, IEventStore store, IDBConnectionFactory dbConnection)
	{
		_logger = logger;
		_store = store;
		_dbConnection = dbConnection;
	}

	public async Task Consume(ConsumeContext<Batch<LocaleItemProjectionMessage>> context)
	{
		await Task.CompletedTask;
		HashSet<string> aggregateIds = new();
		foreach (var msg in context.Message)
		{
			aggregateIds.Add(msg.Message.AggregateId);
		}

		_logger.LogInformation($"message recived: {context.Message.Count()} - num of aggregateIds to process: {aggregateIds.Count()}");

		foreach (var aggregateId in aggregateIds)
		{
			var result = await _store.RetriveByAggregate<BaseLocalePersistenceEvent>(aggregateId);

			if (result.IsError)
			{
				_logger.LogError("Error on doing projection: {}", result.Errors.First().Description);
			}
			else
			{
				var evtList = result.Value;
				_logger.LogInformation($"{evtList.Count()} events processed for {aggregateId}");

				// build aggregate
				var localeItem = new LocaleItem();
				localeItem.Reduce(evtList);
				_logger.LogInformation(localeItem.ToString());

				//create projection

				var sender = await context.GetSendEndpoint(new Uri("queue:projection"));
				var data = JsonParser.Serialize(localeItem);
				if (data.IsError)
				{
					await sender.Send(new LocaleItemUpdatedMessage(localeItem.AggregateId, "updated"));
				}

				var sqlDetail = $"""
					INSERT INTO "localeitem-detail" ("aggregateId", "data")
					VALUES (@AggregateId,@Data)
					ON CONFLICT ("aggregateId")
					DO UPDATE SET "data" = @Data
					WHERE "localeitem-detail"."aggregateId" = @AggregateId;
					""";

				var sqlList = $"""
					INSERT INTO "localeitem-list" ("aggregateId", "lang", "context", "content", "updatedAt", "updatedBy")
					VALUES (@AggregateId,@Lang,@Context,@Content,@UpdatedAt,@UpdatedBy)
					ON CONFLICT ("aggregateId","lang","context")
					DO UPDATE SET "content" = @Content, "updatedAt" = @UpdatedAt, "updatedBy" = @UpdatedBy 
					WHERE "localeitem-list"."aggregateId" = @AggregateId 
					  AND "localeitem-list"."lang" = @Lang 
					  AND "localeitem-list"."context" = @Context;
					""";

				var listItem = new LocaleItemListItem
				(
					AggregateId: localeItem.AggregateId,
					Lang: localeItem.Lang,
					Context: localeItem.Context,
					Content: localeItem.Content,
					UpdatedAt: localeItem.UpdatedAt ?? localeItem.CreatedAt ?? DateTime.UtcNow,
					UpdatedBy: localeItem.UpdatedBy ?? localeItem.CreatedBy
				);


				using (var c = await _dbConnection.CreateConnectionAsync())
				{
					using (var transaction = c.BeginTransaction())
					{
						try
						{
							c.Execute(sqlDetail, new { Data = data.Value, AggregateId = aggregateId });
							c.Execute(sqlList, listItem);
							transaction.Commit();

							_logger.LogInformation("locale item updated");
							await sender.Send(new LocaleItemUpdatedMessage(localeItem.AggregateId, "updated"));
						}
						catch (Exception ex)
						{
							transaction.Rollback();
							_logger.LogError(ex.ToString());
							await sender.Send(new LocaleItemUpdatedMessage(localeItem.AggregateId, "error"));
						}
					}
				}

			}
		}
	}
}
