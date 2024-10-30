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


	public static class DynamoDBStoreEvent
	{
		public static ErrorOr<T> InitFromDynamoDBResult<T>(Dictionary<string, AttributeValue> item)
		{
			var eventDoc = Document.FromAttributeMap(item);
			var evtJson = eventDoc.ToJson();
			var evt = JsonParser.Deserialize<T>(evtJson);
			return evt;
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


		public DynamoDBEventStore(string tableName, string? localhost)
		{
			var opts = new AmazonDynamoDBConfig();
			if (localhost is not null)
			{
				opts.ServiceURL = localhost;
			}
			_client = new AmazonDynamoDBClient(opts);
			_context = new DynamoDBContext(_client);
			_tableName = tableName;
		}

		public async Task InitStore()
		{
			DynamoEventStoreMigration m = new(_client, _tableName);
			var result = await m.CreateTableAsync();
			Console.WriteLine($"db check creation for table {_tableName}: {result}");
		}

		public async Task<ErrorOr<StoreEvent>> Append<T>(T evt) where T : StoreEvent
		{
			var eventAsJson = JsonSerializer.Serialize<T>(evt);
			Console.WriteLine(eventAsJson.ToString());
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

		public async Task<ErrorOr<T>> Retrive<T>(string aggregateId, DateTime createdAt) where T : StoreEvent
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
					var evt = DynamoDBStoreEvent.InitFromDynamoDBResult<T>(resultItem);
					return evt;
				}
				else
				{
					Error.NotFound();
				}
			}

			return Error.Unexpected(code: response.HttpStatusCode.ToString(), description: "Retriving event from store");
		}


		public async Task<ErrorOr<List<T>>> RetriveByAggregate<T>(string aggregateId) where T : StoreEvent
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
				var result = new List<T>();
				foreach (var item in resultItems)
				{
					var evt = DynamoDBStoreEvent.InitFromDynamoDBResult<T>(item);
					if (!evt.IsError)
					{
						result.Add(evt.Value);
					}
					else
					{
						Console.WriteLine("----------------------------");
						Console.WriteLine(evt.Errors.First());
						Console.WriteLine("----------------------------");
					}
				}

				return result;
			}

			return Error.Unexpected(code: response.HttpStatusCode.ToString(), description: "Retriving event from store");
		}

		public async Task StartAsync(CancellationToken cancellationToken)
		{
			await InitStore();
		}

		public async Task StopAsync(CancellationToken cancellationToken)
		{
			await Task.CompletedTask;
		}
	}
}
