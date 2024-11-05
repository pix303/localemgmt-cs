using ErrorOr;

namespace EventSourcingStore;

public class InMemoryEventStore : IEventStore
{

	private List<StoreEvent> _eventList = null!;

	public string GetTableName()
	{
		return "in-memory-default";
	}

	public Task<ErrorOr<StoreEvent>> Append<T>(T @event) where T : StoreEvent
	{
		_eventList.Add(@event);
		var se = @event as StoreEvent;
		return Task.FromResult(se.ToErrorOr());
	}

	public async Task<ErrorOr<T>> Retrive<T>(string aggregateId, DateTime cratedAt) where T : StoreEvent
	{
		await Task.CompletedTask;
		var result = _eventList.Find(item => item.AggregateId == aggregateId && item.CreatedAt == cratedAt) as T;
		if (result is null)
		{
			var err = ErrorOr.Error.NotFound();
			return err;
		}
		return result;
	}

	public async Task<ErrorOr<List<T>>> RetriveByAggregate<T>(string aggregateId) where T : StoreEvent
	{
		await Task.CompletedTask;
		var result = _eventList.FindAll(item => item.AggregateId == aggregateId) as List<T>;
		if (result is not null)
		{
			return result;
		}
		return new List<T>();
	}

	public async Task StartAsync(CancellationToken cancellationToken)
	{
		_eventList = new();
		await Task.CompletedTask;
	}

	public async Task StopAsync(CancellationToken cancellationToken)
	{
		await Task.CompletedTask;
	}

	public Task InitStore()
	{
		_eventList = new();
		return Task.CompletedTask;
	}
}
