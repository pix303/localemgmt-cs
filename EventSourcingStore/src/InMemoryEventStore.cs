using ErrorOr;

namespace EventSourcingStore;

public class InMemoryEventStore : IEventStore
{

	private List<StoreEvent> _eventList = new();

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

	public Task<ErrorOr<T?>> Retrive<T>(string aggregateId, DateTime cratedAt) where T : StoreEvent
	{
		var result = _eventList.Find(item => item.AggregateId == aggregateId && item.CreatedAt == cratedAt) as T;
		return Task.FromResult(result.ToErrorOr());
	}

	public Task<ErrorOr<List<T>>> RetriveByAggregate<T>(string aggregateId) where T : StoreEvent
	{
		var result = _eventList.FindAll(item => item.AggregateId == aggregateId) as List<T>;
		if (result is not null)
		{
			return Task.FromResult(result.ToErrorOr());
		}
		return Task.FromResult(new List<T>().ToErrorOr());
	}
}



