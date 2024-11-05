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
	public NpgsqlDBConnectionFactory(string connectionString, string tableName)
	{
		_connectionString = connectionString;
		_tableName = tableName;
	}

	public async Task<IDbConnection> CreateConnectionAsync(CancellationToken token = default)
	{
		var connection = new NpgsqlConnection(_connectionString);
		await connection.OpenAsync(token);
		return connection;
	}

}