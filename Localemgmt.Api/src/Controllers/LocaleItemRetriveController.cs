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
	[Route("search")]
	public async Task<IActionResult> GetList([FromQuery] LocaleItemListRequest request)
	{
		_logger.LogWarning(request.ToString());
		using (var c = await _dbConnection.CreateConnectionAsync())
		{
			_logger.LogWarning(request.ToString());
			var result = await c.QueryAsync<LocaleItemListItem>($"""
				SELECT * FROM "localeitem-list" 
				WHERE (@Lang IS NULL OR "localeitem-list"."lang" = @Lang)  
				AND (@Context IS NULL OR "localeitem-list"."context" = @Context)  
				AND (@Content IS NULL OR "localeitem-list"."content" LIKE @Content)
			""", request);

			if (result.Count() == 0)
			{
				return Problem(statusCode: StatusCodes.Status404NotFound);
			}

			return Ok(result);
		}
	}


	[HttpGet]
	[Route("detail")]
	public async Task<IActionResult> GetDetail([FromQuery] LocaleItemDetailRequest request)
	{
		using (var c = await _dbConnection.CreateConnectionAsync())
		{
			_logger.LogWarning(request.ToString());
			var result = await c.QuerySingleAsync<LocaleItemDetail>($"""
				SELECT * FROM "localeitem-detail" 
				WHERE "localeitem-detail"."aggregateId" = @AggregateId;
			""", request);

			if (result is null)
			{
				return Problem(statusCode: StatusCodes.Status404NotFound);
			}

			var finalResult = JsonParser.Deserialize<LocaleItem>(result.Data);
			if (finalResult.IsError)
			{
				return Problem(statusCode: StatusCodes.Status500InternalServerError, detail: finalResult.FirstError.ToString());
			}
			return Ok(finalResult.Value);
		}
	}

}
