using EventSourcingStore;
using Localemgmt.Domain.LocaleItems.Events;
using Localemgmt.Domain.LocaleItems.Projections;
using MassTransit;

namespace Localemgmt.Api.Consumer;

public record ProjectionMessage
(
	string AggregateId
);

public class ProjectionConsumerDefinition : ConsumerDefinition<ProjectionConsumer>
{
	public ProjectionConsumerDefinition()
	{
		this.ConcurrentMessageLimit = 15;
	}

	protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<ProjectionConsumer> consumerConfigurator, IRegistrationContext context)
	{
		base.ConfigureConsumer(endpointConfigurator, consumerConfigurator, context);

		endpointConfigurator.Batch<ProjectionMessage>(config =>
		{
			config.MessageLimit = 15;
			config.TimeLimit = TimeSpan.FromSeconds(10);
			config.TimeLimitStart = BatchTimeLimitStart.FromLast;
		});
	}
}


public class ProjectionConsumer : IConsumer<Batch<ProjectionMessage>>
{
	readonly ILogger<ProjectionConsumer> _logger;
	readonly IEventStore _store;

	public ProjectionConsumer(ILogger<ProjectionConsumer> logger, IEventStore store)
	{
		_logger = logger;
		_store = store;
	}

	public async Task Consume(ConsumeContext<Batch<ProjectionMessage>> context)
	{
		await Task.CompletedTask;
		HashSet<string> aggregateIds = new();
		foreach (var msg in context.Message)
		{
			aggregateIds.Add(msg.Message.AggregateId);
		}

		_logger.LogInformation($"msg {context.Message.Count()} - set {aggregateIds.Count()}");

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
				var localeItem = new LocaleItem();
				localeItem.Reduce(evtList);
				_logger.LogInformation(localeItem.ToString());
			}
		}
	}
}
