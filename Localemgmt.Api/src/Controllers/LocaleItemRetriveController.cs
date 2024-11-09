using Microsoft.AspNetCore.Mvc;
using Localemgmt.Contracts.LocaleItem;
using EventSourcingStore;
using Dapper;
using Localemgmt.Domain.LocaleItems.Projections;

namespace Localemgmt.Api.Controllers;

[ApiController]
[Route("localeitem")]
public class LocaleItemRetriveController : ControllerBase
{
	ILogger<LocaleItemRetriveController> _logger;
	IDBConnectionFactory _dbConnection;


	public LocaleItemRetriveController(
		ILogger<LocaleItemRetriveController> logger,
		IDBConnectionFactory connection
	)
	{
		_logger = logger;
		_dbConnection = connection;
	}


	[HttpGet]
	[Route("context/{context}")]
	public async Task<IActionResult> GetContext([FromRoute] string context, [FromQuery] string? lang)
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

			if (result.Count() == 0)
			{
				return Problem(statusCode: StatusCodes.Status404NotFound);
			}

			return Ok(result);
		}
	}


	[HttpGet]
	[Route("detail/{aggregateId}")]
	public async Task<IActionResult> GetDetail(string aggregateId)
	{
		using (var c = await _dbConnection.CreateConnectionAsync())
		{
			var result = await c.QuerySingleAsync<LocaleItemDetail>($"""
				SELECT * FROM "localeitem-detail" 
				WHERE "localeitem-detail"."aggregateId" = @AggregateId;
			""", new { aggregateId });

			if (result is null)
			{
				return Problem(statusCode: StatusCodes.Status404NotFound);
			}

			var finalResult = JsonParser.Deserialize<LocaleItemAggregate>(result.Data);
			if (finalResult.IsError)
			{
				return Problem(statusCode: StatusCodes.Status500InternalServerError, detail: finalResult.FirstError.ToString());
			}
			return Ok(finalResult.Value);
		}
	}

	[HttpGet]
	[Route("search")]
	public async Task<IActionResult> Search([FromQuery] LocaleItemSearchRequest request)
	{
		request.content = $"%{request.content}%";

		Console.WriteLine(request.content);
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
				 request);

				if (result.Count() == 0)
				{
					return Problem(statusCode: StatusCodes.Status404NotFound);
				}

				return Ok(result.ToList());

			}
			catch (Exception ex)
			{
				return Problem(statusCode: StatusCodes.Status500InternalServerError, detail: ex.ToString());
			}
		}
	}


	[HttpGet]
	[Route("match")]
	public async Task<IActionResult> Match([FromQuery] LocaleItemSearchRequest request)
	{
		using (var c = await _dbConnection.CreateConnectionAsync())
		{
			try
			{
				var result = await c.QueryFirstAsync<string>($"""
					SELECT "aggregateId" FROM "localeitem-list" 
					WHERE (@lang IS NULL OR "localeitem-list"."lang" = @lang)
					AND (@context IS NULL OR "localeitem-list"."context" = @context)
					AND (@content IS NULL OR "localeitem-list"."content" = @content)
				""",
				 request);

				if (result is null)
				{
					return Problem(statusCode: StatusCodes.Status404NotFound);
				}

				return await this.GetDetail(result);

			}
			catch (Exception ex)
			{
				return Problem(statusCode: StatusCodes.Status500InternalServerError, detail: ex.ToString());
			}
		}
	}
}
