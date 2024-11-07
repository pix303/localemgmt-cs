using System.Data;
using Npgsql;

namespace EventSourcingStore;

public interface IDBConnectionFactory
{
	Task<IDbConnection> CreateConnectionAsync(CancellationToken token = default);
}

public class NpgsqlDBConnectionFactory : IDBConnectionFactory
{
	private readonly string _connectionString;
	private readonly string _tableName;
	public NpgsqlDBConnectionFactory(StoreSettings settings)
	{
		_tableName = settings.TableName;
		if (settings.Connection is not null)
		{
			_connectionString = settings.Connection;
		}
		else
		{
			throw new Exception("missing postgresql connection string");
		}
	}

	public async Task<IDbConnection> CreateConnectionAsync(CancellationToken token = default)
	{
		var connection = new NpgsqlConnection(_connectionString);
		await connection.OpenAsync(token);
		return connection;
	}

}
