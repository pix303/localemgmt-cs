using EventSourcingStore;

namespace Localemgmt.Domain;

public interface IAggregate<TEvent> where TEvent : StoreEvent
{
	void Apply(TEvent @event);
	void Reduce(IList<TEvent> @events);
}
