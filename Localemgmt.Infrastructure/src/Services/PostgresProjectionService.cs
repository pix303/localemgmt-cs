using Dapper;
using ErrorOr;
using EventSourcingStore;
using Localemgmt.Contracts.LocaleItem;
using Localemgmt.Domain.LocaleItems.Projections;
namespace Localemgmt.Infrastructure.Services;


public class PostgresProjectionService : IRetriveService
{

	private IDBConnectionFactory _dbConnection;

	public PostgresProjectionService(IDBConnectionFactory dbConnection)
	{
		_dbConnection = dbConnection;
	}


	public async Task<ErrorOr<LocaleItemAggregate>> GetDetail(string aggregateId)
	{
		using (var c = await _dbConnection.CreateConnectionAsync())
		{
			var result = await c.QuerySingleAsync<LocaleItemDetail>($"""
				SELECT * FROM "localeitem-detail" 
				WHERE "localeitem-detail"."aggregateId" = @AggregateId;
			""", new { aggregateId });

			if (result is null)
			{
				return Error.NotFound();
			}

			var finalResult = JsonParser.Deserialize<LocaleItemAggregate>(result.Data);
			if (finalResult.IsError)
			{
				return Error.Failure(description: finalResult.FirstError.ToString());
			}
			return finalResult.Value;
		}
	}


	public async Task<ErrorOr<List<LocaleItemListItem>>> Search(LocaleItemSearchRequest request)
	{
		if (request.content is not null)
		{
			request.content = $"%{request.content}%";
		}

		using (var c = await _dbConnection.CreateConnectionAsync())
		{
			try
			{
				var result = await c.QueryAsync<LocaleItemListItem>($"""
					SELECT * FROM "localeitem-list" 
					WHERE (@lang IS NULL OR "localeitem-list"."lang" = @lang)
					AND (@context IS NULL OR "localeitem-list"."context" = @context)
					AND (@content IS NULL OR "localeitem-list"."content" LIKE @content) 
					""",
				request
				);

				if (result is null)
				{
					return ErrorOr.Error.NotFound();
				}

				return result.ToList();

			}
			catch (Exception ex)
			{
				return Error.Failure(description: ex.ToString());
			}
		}
	}

	public async Task<ErrorOr<LocaleItemAggregate>> Match(LocaleItemSearchRequest request)
	{
		using (var c = await _dbConnection.CreateConnectionAsync())
		{
			try
			{
				var result = await c.QueryFirstOrDefaultAsync<string>($"""
					SELECT "aggregateId" FROM "localeitem-list" 
					WHERE (@lang IS NULL OR "localeitem-list"."lang" = @lang)
					AND (@context IS NULL OR "localeitem-list"."context" = @context)
					AND (@content IS NULL OR "localeitem-list"."content" = @content) 
					""",
				request
				);

				if (result is null)
				{
					return ErrorOr.Error.NotFound();
				}

				return await GetDetail(result);

			}
			catch (Exception ex)
			{
				return Error.Failure(description: ex.ToString());
			}
		}
	}

	public async Task<ErrorOr<List<LocaleItemListItem>>> GetContext(string context, string? lang)
	{
		using (var c = await _dbConnection.CreateConnectionAsync())
		{
			var result = await c.QueryAsync<LocaleItemListItem>($""" 
			SELECT * FROM "localeitem-list" 
			WHERE "localeitem-list"."context" = @context 
			AND (@lang IS NULL OR lang = @lang)
			""",
			new { context, lang }
			);

			if (result is null || result.Count() == 0)
			{
				return Error.NotFound();
			}

			return result.ToList();
		}
	}

}
