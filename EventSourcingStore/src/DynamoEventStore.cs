using System.Text.Json;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using ErrorOr;

namespace EventSourcingStore
{
	public static class DyanamoDateFormatters
	{
		public const string DATETIME_FORMATTER = "yyyy-MM-dd'T'HH:mm:ss.fffffffK";

		public static string FormatDate(DateTime date)
		{
			var dateKey = date.ToString(DyanamoDateFormatters.DATETIME_FORMATTER);
			return dateKey;
		}
	}


	public class DynamoDBStoreEvent : StoreEvent
	{
		public DynamoDBStoreEvent() : base()
		{ }

		public DynamoDBStoreEvent(Dictionary<string, AttributeValue> item)
		{
			this.InitFromDynamoDBResult(item);
		}

		public void InitFromDynamoDBResult(Dictionary<string, AttributeValue> item)
		{
			var eventDoc = Document.FromAttributeMap(item);

			DynamoDBEntry creationDateEntry;
			eventDoc.TryGetValue("createdAt", out creationDateEntry);
			DynamoDBEntry id;
			eventDoc.TryGetValue("id", out id);
			DynamoDBEntry aggregateId;
			eventDoc.TryGetValue("aggregateId", out aggregateId);

			var evtJson = eventDoc.ToJson();
			var evt = JsonSerializer.Deserialize<StoreEvent>(evtJson);
			var creationDate = creationDateEntry.AsDateTime();
			this.InitEvent(id, DateTime.UtcNow, aggregateId);
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

		public async Task<ErrorOr<StoreEvent>> Append<T>(T evt) where T : StoreEvent
		{
			evt.InitEvent();
			var eventAsJson = JsonSerializer.Serialize<T>(evt);
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
				return evt;
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
							S = DyanamoDateFormatters.FormatDate(createdAt)
						}
					}
				}
			};

			var response = await _client.GetItemAsync(getEventRequest);
			if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
			{
				var resultItem = response.Item;

				if (resultItem is not null && resultItem.Count > 0)
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
