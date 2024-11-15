using Microsoft.AspNetCore.Mvc;
using Localemgmt.Contracts.LocaleItem;
using Localemgmt.Infrastructure.Services;

namespace Localemgmt.Api.Controllers;

[ApiController]
[Route("localeitem")]
public class LocaleItemRetriveController : ControllerBase
{
	ILogger<LocaleItemRetriveController> _logger;
	IRetriveService _service;


	public LocaleItemRetriveController(
		ILogger<LocaleItemRetriveController> logger,
		IRetriveService service
	)
	{
		_logger = logger;
		_service = service;
	}


	[HttpGet]
	[Route("context/{context}")]
	public async Task<IActionResult> GetContext([FromRoute] string context, [FromQuery] string? lang)
	{
		var result = await _service.GetContext(context, lang);
		return result.MatchFirst(
			value => Ok(value),
			err =>
			{
				var statusCode = err.Code == ErrorOr.ErrorType.NotFound.ToString() ? StatusCodes.Status404NotFound : StatusCodes.Status500InternalServerError;
				return Problem(statusCode: statusCode, detail: err.Description);
			}
		);
	}


	[HttpGet]
	[Route("detail/{aggregateId}")]
	public async Task<IActionResult> GetDetail(string aggregateId)
	{
		var result = await _service.GetDetail(aggregateId);
		return result.MatchFirst(
			value => Ok(value),
			err =>
			{
				var statusCode = err.Code == ErrorOr.ErrorType.NotFound.ToString() ? StatusCodes.Status404NotFound : StatusCodes.Status500InternalServerError;
				return Problem(statusCode: statusCode, detail: err.Description);
			}
		);
	}


	[HttpGet]
	[Route("search")]
	public async Task<IActionResult> Search([FromQuery] LocaleItemSearchRequest request)
	{
		var result = await _service.Search(request);
		return result.MatchFirst(
			value => Ok(value),
			err =>
			{
				var statusCode = err.Code == ErrorOr.ErrorType.NotFound.ToString() ? StatusCodes.Status404NotFound : StatusCodes.Status500InternalServerError;
				return Problem(statusCode: statusCode, detail: err.Description);
			}
		);
	}


	[HttpGet]
	[Route("match")]
	public async Task<IActionResult> Match([FromQuery] LocaleItemSearchRequest request)
	{
		var result = await _service.Match(request);
		return result.MatchFirst(
			value => Ok(value),
			err =>
			{
				var statusCode = err.Code.Contains(ErrorOr.ErrorType.NotFound.ToString()) ? StatusCodes.Status404NotFound : StatusCodes.Status500InternalServerError;
				return Problem(statusCode: statusCode, detail: err.Description);
			}
		);
	}
}
