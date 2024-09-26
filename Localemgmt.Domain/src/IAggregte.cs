using EventSourcingStore;

namespace Localemgmt.Domain;

public interface IAggregate
{
	void Apply(StoreEvent @event);
	void Reduce(IList<StoreEvent> @events);
}
