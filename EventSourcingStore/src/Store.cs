using System.Text.Json.Serialization;

namespace EventSourcingStore;


public class StoreEvent
{
	[JsonPropertyName("id")]
	public string Id { get; protected set; }
	[JsonPropertyName("aggregateId")]
	public string AggregateId { get; set; } = "";
	[JsonPropertyName("createdAt")]
	public DateTime CreatedAt { get; protected set; }
	[JsonPropertyName("type")]
	public string Type { get; protected set; } = "NO-TYPE";

	public StoreEvent()
	{
		Id = "";
		this.InitEvent();
	}

	override public string ToString()
	{
		return $"id: {Id} - aggregateId: {AggregateId} - createdAt: {CreatedAt}";
	}

	public void InitEvent()
	{
		this.Id = Guid.NewGuid().ToString();
		this.CreatedAt = DateTime.UtcNow;
	}

	public void InitEvent(string id, DateTime createdAt)
	{
		this.Id = id;
		this.CreatedAt = createdAt;
	}

}


public interface IEventStore
{
	Task<bool> Append<T>(T @event) where T : StoreEvent;
	Task<StoreEvent?> Retrive(string aggregateId, DateTime cratedAt);
	Task<List<StoreEvent>> RetriveByAggregate(string aggregateId);
	string GetTableName();
}


