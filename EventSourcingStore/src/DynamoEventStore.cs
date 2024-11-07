using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using ErrorOr;
using Microsoft.Extensions.Options;

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
		private readonly StoreSettings _settings = null!;

		public string GetTableName()
		{
			return _settings.TableName;
		}


		public DynamoDBEventStore(StoreSettings settings)
		{
			_settings = settings;
			var opts = new AmazonDynamoDBConfig();
			if (_settings.Connection is not null)
			{
				opts.ServiceURL = _settings.Connection;
			}
			_client = new AmazonDynamoDBClient(opts);
			_context = new DynamoDBContext(_client);
		}

		public async Task InitStore()
		{
			DynamoEventStoreMigration m = new(_client, _settings.TableName);
			var result = await m.CreateTableAsync();
			Console.WriteLine($"db check creation for table {_settings.TableName}: {result}");
		}

		public async Task<ErrorOr<StoreEvent>> Append<T>(T evt) where T : StoreEvent
		{
			var eventAsJson = JsonParser.Serialize(evt);
			if (eventAsJson.IsError)
			{
				return Error.Unexpected(description: "Error on serialize event");
			}

			var eventAsDoc = Document.FromJson(eventAsJson.Value);
			var eventAsAttributes = eventAsDoc.ToAttributeMap();

			var appendEventRequest = new PutItemRequest
			{
				TableName = _settings.TableName,
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
				TableName = _settings.TableName,
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
				TableName = _settings.TableName,
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
						Console.WriteLine(evt.Errors.First());
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
