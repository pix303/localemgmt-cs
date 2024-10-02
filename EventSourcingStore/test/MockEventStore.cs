using ErrorOr;

namespace EventSourcingStore.Test
{

	public class MockEventStore : IEventStore
	{

		private List<StoreEvent> _eventList = new();

		public string GetTableName()
		{
			return "unit-test-default";
		}

		public Task<ErrorOr<StoreEvent>> Append<T>(T @event) where T : StoreEvent
		{
			_eventList.Add(@event);
			var se = @event as StoreEvent;
			return Task.FromResult(se.ToErrorOr());
		}

		public Task<ErrorOr<StoreEvent?>> Retrive(string aggregateId, DateTime cratedAt)
		{
			var result = _eventList.Find(item => item.AggregateId == aggregateId && item.CreatedAt == cratedAt);
			return Task.FromResult(result.ToErrorOr());
		}

		public Task<ErrorOr<List<StoreEvent>>> RetriveByAggregate(string aggregateId)
		{
			var result = _eventList.FindAll(item => item.AggregateId == aggregateId);
			if (result is not null)
			{
				return Task.FromResult(result.ToErrorOr());
			}
			return Task.FromResult(new List<StoreEvent>().ToErrorOr());
		}
	}

}

