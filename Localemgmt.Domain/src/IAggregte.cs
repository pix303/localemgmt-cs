namespace Localemgmt.Domain;

public interface IAggregate
{
	void Apply(EventBase @event);
	void Reduce(IList<EventBase> @events);
}
