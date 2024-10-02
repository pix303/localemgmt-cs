using Microsoft.AspNetCore.Mvc;
using MediatR;
using Mapster;
using Localemgmt.Contracts.LocaleItem;
using Localemgmt.Application.LocaleItem.Commands.Add;
using Localemgmt.Application.LocaleItem.Commands.Update;
using Localemgmt.Application.Commons;


namespace Localemgmt.Api.Controllers;

[ApiController]
[Route("localeitem")]
public class LocaleItemMutationController : ControllerBase
{
	ISender _mediator;


	public LocaleItemMutationController(ISender mediator)
	{
		_mediator = mediator;
	}


	[HttpPost]
	[Route("add")]
	public Task<IActionResult> Add(LocaleItemMutationRequest request)
	{
		return Mutation<AddLocaleItemCommand>(request);
	}


	[HttpPost]
	[Route("update")]
	public Task<IActionResult> Update(LocaleItemMutationRequest request)
	{
		return Mutation<UpdateLocaleItemCommand>(request);
	}

	private async Task<IActionResult> Mutation<TCommand>(LocaleItemMutationRequest request) where TCommand : ICommand
	{
		var command = request.Adapt<TCommand>();

		var validationResult = command.Validate();
		if (validationResult.IsError)
		{
			return Problem(validationResult.FirstError.Description, null, StatusCodes.Status400BadRequest, "Bad request");
		}

		var serviceResult = await _mediator.Send(command);
		return serviceResult.Match(
		  value => Ok(new LocaleItemMutationResponse(value.AggregateId)),
		  exp => Problem(exp.First().Description)
		);
	}
}
