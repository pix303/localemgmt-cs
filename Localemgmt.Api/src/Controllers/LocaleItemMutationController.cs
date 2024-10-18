using Microsoft.AspNetCore.Mvc;
using Mapster;
using Localemgmt.Contracts.LocaleItem;
using EventSourcingStore;
using Localemgmt.Domain.LocaleItems.Events;
using Localemgmt.Application.LocaleItem.Validators;
using FluentValidation;
using MassTransit;
using Localemgmt.Api.Consumer;

namespace Localemgmt.Api.Controllers;

[ApiController]
[Route("localeitem")]
public class LocaleItemMutationController : ControllerBase
{
	IEventStore _store;
	ILogger<LocaleItemMutationController> _logger;
	IPublishEndpoint? _bus;


	public LocaleItemMutationController(
		IEventStore store,
		ILogger<LocaleItemMutationController> logger,
		IPublishEndpoint? bus
	)
	{
		_store = store;
		_logger = logger;
		_bus = bus;
	}


	[HttpPost]
	[Route("add")]
	public async Task<IActionResult> Add(LocaleItemCreationRequest request)
	{
		_logger.LogInformation("Adding locale item by request {}", request);

		// event validation
		var validationError = Validate<LocaleItemCreationRequest>(new AddLocaleItemRequestValidator(), request);
		if (validationError is not null)
		{
			_logger.LogError("Error on validation {}", validationError);
			return validationError;
		}

		// event persistence
		var e = request.Adapt<LocaleItemCreationEvent>();
		var result = await _store.Append<LocaleItemCreationEvent>(e);

		if (result.IsError)
		{
			return Problem(result.Errors.First().Description, null, StatusCodes.Status500InternalServerError);
		}

		var aggregateId = result.Value.AggregateId;
		if (aggregateId is null)
		{
			return Problem("aggregateId cant be null", null, StatusCodes.Status500InternalServerError);
		}

		if (_bus is not null)
		{
			var msg = new ProjectionMessage(aggregateId);
			await _bus.Publish(msg);
		}

		return Ok(new LocaleItemMutationResponse(aggregateId));
	}


	[HttpPost]
	[Route("update")]
	public async Task<IActionResult> Update(LocaleItemUpdateRequest request)
	{
		// event validation
		var validationError = Validate<LocaleItemUpdateRequest>(new UpdateLocaleItemRequestValidator(), request);
		if (validationError is not null)
		{
			return validationError;
		}

		// event persistence
		var e = request.Adapt<LocaleItemUpdateEvent>();
		var result = await _store.Append<LocaleItemUpdateEvent>(e);

		if (result.IsError)
		{
			return Problem(result.Errors.First().Description, null, StatusCodes.Status500InternalServerError, "Internal server error");
		}

		var aggregateId = result.Value.AggregateId;
		var msg = new ProjectionMessage(aggregateId);
		if (_bus is not null)
		{
			await _bus.Publish(msg);
		}
		return Ok(new LocaleItemMutationResponse(aggregateId));
	}


	private ObjectResult? Validate<T>(IValidator<T> validator, T request)
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
