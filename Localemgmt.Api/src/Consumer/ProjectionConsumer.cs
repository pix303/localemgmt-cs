using MassTransit;

namespace Localemgmt.Api.Consumer;

public record ProjectionMessage
(
	string AggregateId
);

public class ProjectionConsumer : IConsumer<ProjectionMessage>
{
	readonly ILogger<ProjectionConsumer> _logger;

	public ProjectionConsumer(ILogger<ProjectionConsumer> logger)
	{
		_logger = logger;
	}

	public async Task Consume(ConsumeContext<ProjectionMessage> context)
	{
		await Task.Delay(5000);
		_logger.LogWarning("------------------- qualcosa {}", context.Message.AggregateId);
	}
}
