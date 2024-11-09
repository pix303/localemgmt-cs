using ErrorOr;
using Dapper;


namespace EventSourcingStore;

public class PostgresEventStore : IEventStore
{
	private readonly IDBConnectionFactory _dbConnector;
	private readonly string _tableName;

	public string GetTableName()
	{
		return _tableName;
	}

	public PostgresEventStore(IDBConnectionFactory connection, string tableName)
	{
		_dbConnector = connection;
		_tableName = tableName;
	}

	public async Task InitStore()
	{
		// ----------------------------------------------------------------------x
		// TODO: spostare in altra lib init tabelle di projection
		// ----------------------------------------------------------------------x
		using (var c = await _dbConnector.CreateConnectionAsync())
		{
			var sqlStatement = $"""
			CREATE TABLE IF NOT EXISTS public."{_tableName}"
			(
			    "id" character varying(64) NOT NULL,
			    "aggregateId" character varying(64) NOT NULL,
			    "createdAt" character varying(64) NOT NULL,
			    "userId" character varying(128)  NOT NULL,
			    "eventType" character varying(64) NOT NULL,
			    "data" text,
			    CONSTRAINT "localemgmt-store_pkey" PRIMARY KEY (id),
			    CONSTRAINT "localemgmt-store_ukey" UNIQUE ("aggregateId", "createdAt")
			);

			CREATE TABLE IF NOT EXISTS public."localeitem-detail"
			(
			    "aggregateId" character varying(64) NOT NULL,
			    "data" text,
			    CONSTRAINT "localeitem-detail_pkey" PRIMARY KEY ("aggregateId")
			);

			CREATE TABLE IF NOT EXISTS public."localeitem-list"
			(
			    "aggregateId" character varying(64) NOT NULL,
			    "lang" character varying(8) NOT NULL,
			    "context" character varying(64) NOT NULL,
			    "content" text NOT NULL,
			    "updatedAt" character varying(64) NOT NULL,
			    "updatedBy" character varying(64) NOT NULL,
			    CONSTRAINT "localeitem-list_ukey" UNIQUE ("aggregateId","lang","context")
			);
			""";

			await c.ExecuteAsync(sqlStatement);
		}
	}


	public async Task<ErrorOr<StoreEvent>> Append<T>(T evt) where T : StoreEvent
	{
		var data = JsonParser.Serialize(evt);
		Console.WriteLine(evt);
		// TODO: handle error
		evt.Data = data.Value;
		var s = $"""INSERT INTO "{_tableName}" ("id", "aggregateId", "createdAt", "userId", "eventType","data") VALUES (@Id,@AggregateId,@CreatedAt,@UserId,@EventType,@Data)""";
		using (var c = await _dbConnector.CreateConnectionAsync())
		{
			var result = await c.ExecuteAsync(s, evt);
			evt.Data = null;
			return evt;
		}
	}


	public async Task<ErrorOr<T>> Retrive<T>(string aggregateId, DateTime createdAt) where T : StoreEvent
	{
		var p = new { aggregateId, createdAt = createdAt.ToFileTimeUtc() };
		using (var c = await _dbConnector.CreateConnectionAsync())
		{
			var row = await c.QuerySingleAsync<string>($"""SELECT data FROM "{_tableName}" WHERE "aggregateId" = @aggregateId AND "createdAt" = @createdAt """, p);
			return DeserializeRow<T>(row);
		}
	}

	public async Task<ErrorOr<List<T>>> RetriveByAggregate<T>(string aggregateId) where T : StoreEvent
	{
		var p = new { aggregateId };
		List<T> result = new();

		using (var c = await _dbConnector.CreateConnectionAsync())
		{
			var rows = await c.QueryAsync<string>($"SELECT data FROM \"{_tableName}\" WHERE \"aggregateId\" = @aggregateId", p);
			foreach (var row in rows)
			{
				var evt = DeserializeRow<T>(row);

				if (!evt.IsError)
				{
					result.Add(evt.Value);
				}
				else
				{
					ErrorOr.Error.Failure(description: evt.Errors.First().ToString());
				}
			}

			if (result.Count() == 0)
			{
				// TODO better describe error
				return ErrorOr.Error.NotFound();
			}
			return result;
		}
	}


	private ErrorOr<T> DeserializeRow<T>(string? rowData)
	{
		if (rowData is null)
		{
			// TODO better describe error
			return ErrorOr.Error.NotFound();
		}

		var result = JsonParser.Deserialize<T>(rowData);
		if (result.IsError)
		{
			// TODO better describe error
			return ErrorOr.Error.Failure();
		}

		return result.Value;
	}

	public async Task StartAsync(CancellationToken cancellationToken)
	{
		await InitStore();
	}

	public Task StopAsync(CancellationToken cancellationToken)
	{
		return Task.CompletedTask;
	}
}

