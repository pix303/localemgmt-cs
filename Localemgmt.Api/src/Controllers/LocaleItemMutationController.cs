using Microsoft.AspNetCore.Mvc;
using Mapster;
using Localemgmt.Contracts.LocaleItem;
using EventSourcingStore;
using Localemgmt.Domain.LocaleItems.Events;
using Localemgmt.Application.LocaleItem.Validators;
using FluentValidation;

namespace Localemgmt.Api.Controllers;

[ApiController]
[Route("localeitem")]
public class LocaleItemMutationController : ControllerBase
{
	IEventStore _store;
	ILogger<LocaleItemMutationController> _logger;

	public LocaleItemMutationController(IEventStore store, ILogger<LocaleItemMutationController> logger)
	{
		_store = store;
		_logger = logger;
	}


	[HttpPost]
	[Route("add")]
	public async Task<IActionResult> Add(LocaleItemMutationRequest request)
	{
		_logger.LogInformation("Adding locale item by request {}", request);

		// event validation
		var validationError = Validate(new AddLocaleItemRequestValidator(), request);
		if (validationError is not null)
		{
			_logger.LogError("Error on validation {}", validationError);
			return validationError;
		}

		// event persistence
		var e = request.Adapt<LocaleItemCreationEvent>();
		var result = await _store.Append(e);
		return result.Match(
			value =>
			{
				_logger.LogInformation("Added locale item with id {}", value.AggregateId);
				return Ok(new LocaleItemMutationResponse(value.AggregateId));
			},
			err => Problem(err.First().Description, null, StatusCodes.Status500InternalServerError, "Internal server error")
	  );
	}


	[HttpPost]
	[Route("update")]
	public async Task<IActionResult> Update(LocaleItemMutationRequest request)
	{
		// event validation
		var validationError = Validate(new UpdateLocaleItemRequestValidator(), request);
		if (validationError is not null)
		{
			return validationError;
		}

		// event persistence
		var e = request.Adapt<LocaleItemUpdateEvent>();
		var result = await _store.Append(e);
		return result.Match(
			value => Ok(new LocaleItemMutationResponse(value.AggregateId)),
			err => Problem(err.First().Description, null, StatusCodes.Status500InternalServerError, "Internal server error")
	  );
	}


	private ObjectResult? Validate(IValidator<LocaleItemMutationRequest> validator, LocaleItemMutationRequest request)
	{
		// event validation
		var validation = validator.Validate(request);
		if (validation.IsValid == false)
		{
			var errorMsg = validation.Errors.First().ErrorMessage;
			return Problem(errorMsg, null, StatusCodes.Status400BadRequest, "Bad request");
		}
		return null;
	}
}
