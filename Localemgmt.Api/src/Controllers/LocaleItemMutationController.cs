using Microsoft.AspNetCore.Mvc;
using ErrorOr;
using MediatR;
using Localemgmt.Contracts.LocaleItem;
using Mapster;
using Localemgmt.Application.LocaleItem.Commands.Add;
using Localemgmt.Application.LocaleItem.Commands.Update;


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
	public Task<ActionResult<LocaleItemMutationResponse>> Add(LocaleItemMutationRequest request)
	{
		return Mutation<AddLocaleItemCommand>(request);
	}


	[HttpPost]
	[Route("update")]
	public Task<ActionResult<LocaleItemMutationResponse>> Update(LocaleItemMutationRequest request)
	{
		return Mutation<UpdateLocaleItemCommand>(request);
	}

	private async Task<ActionResult<LocaleItemMutationResponse>> Mutation<TCommand>(LocaleItemMutationRequest request) where TCommand : IRequest<ErrorOr<bool>>
	{
		var command = request.Adapt<TCommand>();
		ErrorOr<bool> serviceResult = await _mediator.Send(command);
		return serviceResult.Match(
		  value => Ok(new LocaleItemMutationResponse(value)),
		  exp => Problem(exp.First().Description)
		);
	}



}
