using System.Text.Json;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using ErrorOr;

namespace EventSourcingStore
{
	static class Formatters
	{
		public const string DATETIME_FORMATTER = "yyyy-MM-dd'T'HH:mm:ss.fffffffK";
	}

	public class DynamoDBStoreEvent : StoreEvent
	{
		public DynamoDBStoreEvent() : base()
		{ }

		public DynamoDBStoreEvent(Dictionary<string, AttributeValue> item) : base()
		{
			this.InitFromDynamoDBResult(item);
		}

		public void InitFromDynamoDBResult(Dictionary<string, AttributeValue> item)
		{
			var eventDoc = Document.FromAttributeMap(item);

			DynamoDBEntry creationDate;
			eventDoc.TryGetValue("createdAt", out creationDate);
			DynamoDBEntry id;
			eventDoc.TryGetValue("id", out id);

			var eventJson = eventDoc.ToJson();
			var @event = JsonSerializer.Deserialize<StoreEvent>(eventJson);

			if (@event is not null)
			{
				this.AggregateId = @event.AggregateId;
				this.InitEvent(id.AsString(), creationDate.AsDateTimeUtc());
			}
		}
	}

	public class DynamoDBEventStore : IEventStore
	{
		private readonly AmazonDynamoDBClient _client;
		private readonly DynamoDBContext _context;
		private readonly string _tableName;

		public string GetTableName()
		{
			return _tableName;
		}


		public DynamoDBEventStore(string tableName)
		{
			_client = new AmazonDynamoDBClient();
			_context = new DynamoDBContext(_client);
			_tableName = tableName;
		}

		public async Task<ErrorOr<StoreEvent>> Append<T>(T @event) where T : StoreEvent
		{
			@event.InitEvent();
			var eventAsJson = JsonSerializer.Serialize<T>(@event);
			var eventAsDoc = Document.FromJson(eventAsJson);
			var eventAsAttributes = eventAsDoc.ToAttributeMap();

			var appendEventRequest = new PutItemRequest
			{
				TableName = _tableName,
				Item = eventAsAttributes
			};

			var response = await _client.PutItemAsync(appendEventRequest);

			if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
			{
				return @event;
			}

			return Error.Unexpected(code: response.HttpStatusCode.ToString(), description: "Appending event to store");
		}

		public async Task<ErrorOr<StoreEvent?>> Retrive(string aggregateId, DateTime createdAt)
		{
			var getEventRequest = new GetItemRequest
			{
				TableName = _tableName,
				Key = new Dictionary<string, AttributeValue>()
				{
					{
						"aggregateId",
						new AttributeValue
						{
							S = aggregateId
						}
					},
					{
						"createdAt",
						new AttributeValue
						{
							S = createdAt.ToString(Formatters.DATETIME_FORMATTER)
						}
					}
				}
			};

			var response = await _client.GetItemAsync(getEventRequest);
			if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
			{
				var resultItem = response.Item;

				if (resultItem is not null)
				{
					var evt = new DynamoDBStoreEvent(resultItem);
					return evt;
				}
				else
				{
					Error.NotFound();
				}
			}

			return Error.Unexpected(code: response.HttpStatusCode.ToString(), description: "Retriving event from store");
		}


		public async Task<ErrorOr<List<StoreEvent>>> RetriveByAggregate(string aggregateId)
		{
			var q = new QueryRequest
			{
				TableName = _tableName,
				KeyConditionExpression = "aggregateId = :aId",
				ExpressionAttributeValues = new Dictionary<string, AttributeValue>()
				{
					{
						":aId",
						new AttributeValue
						{
							S = aggregateId
						}
					}
				}
			};

			var response = await _client.QueryAsync(q);
			if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
			{
				var resultItems = response.Items;

				var result = new List<StoreEvent>();
				foreach (var item in resultItems)
				{
					var evt = new DynamoDBStoreEvent(item);
					result.Add(evt);
				}

				return result;
			}

			return Error.Unexpected(code: response.HttpStatusCode.ToString(), description: "Retriving event from store");
		}
	}
}
