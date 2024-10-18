using System.Text.Json.Serialization;
using ErrorOr;

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
		this.Id = Guid.NewGuid().ToString();
		this.CreatedAt = DateTime.UtcNow;
		if (String.IsNullOrEmpty(this.AggregateId))
		{
			this.AggregateId = Guid.NewGuid().ToString();
		}
	}

	public StoreEvent(string aggregateId)
	{
		this.Id = Guid.NewGuid().ToString();
		this.CreatedAt = DateTime.UtcNow;
		this.AggregateId = aggregateId;
	}

	[JsonConstructor]
	private StoreEvent(string id, DateTime createdAt, string aggregateId, string type)
	{
		this.Id = id;
		this.CreatedAt = createdAt;
		this.AggregateId = aggregateId;
		this.Type = type;
	}

	override public string ToString()
	{
		return $"id: {Id} - aggregateId: {AggregateId} - createdAt: {CreatedAt} - type: {Type}";
	}
}



public interface IEventStore
{
	Task<ErrorOr<StoreEvent>> Append<T>(T @event) where T : StoreEvent;
	Task<ErrorOr<T>> Retrive<T>(string aggregateId, DateTime cratedAt) where T : StoreEvent;
	Task<ErrorOr<List<T>>> RetriveByAggregate<T>(string aggregateId) where T : StoreEvent;
	string GetTableName();
}


