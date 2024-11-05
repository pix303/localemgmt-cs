using System.Text.Json.Serialization;
using ErrorOr;
using Microsoft.Extensions.Hosting;


namespace EventSourcingStore;


public class StoreEvent
{
	[JsonPropertyName("id")]
	public string Id { get; protected set; }
	[JsonPropertyName("aggregateId")]
	public string AggregateId { get; set; } = "";
	[JsonPropertyName("createdAt")]
	public DateTime CreatedAt { get; protected set; }
	[JsonPropertyName("eventType")]
	public string EventType { get; protected set; } = "NO-TYPE";
	[JsonPropertyName("data")]
	public string? Data { get; set; } = null;

	public StoreEvent()
	{
	}

	public StoreEvent(string eventType)
	{
		this.Id = Guid.NewGuid().ToString();
		this.CreatedAt = DateTime.UtcNow;
		this.AggregateId = Guid.NewGuid().ToString();
		this.EventType = eventType;
	}

	public StoreEvent(string eventType, string aggregateId)
	{
		this.Id = Guid.NewGuid().ToString();
		this.CreatedAt = DateTime.UtcNow;
		this.AggregateId = aggregateId;
		this.EventType = eventType;
	}

	[JsonConstructor]
	public StoreEvent(string id, DateTime createdAt, string aggregateId, string eventType)
	{
		this.Id = id;
		this.CreatedAt = createdAt;
		this.AggregateId = aggregateId;
		this.EventType = eventType;
	}

	override public string ToString()
	{
		return $"id: {Id} - aggregateId: {AggregateId} - createdAt: {CreatedAt} - type: {EventType}";
	}
}



public interface IEventStore : IHostedService
{
	Task InitStore();
	Task<ErrorOr<StoreEvent>> Append<T>(T @event) where T : StoreEvent;
	Task<ErrorOr<T>> Retrive<T>(string aggregateId, DateTime cratedAt) where T : StoreEvent;
	Task<ErrorOr<List<T>>> RetriveByAggregate<T>(string aggregateId) where T : StoreEvent;
	string GetTableName();
}


