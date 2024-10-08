using Microsoft.AspNetCore.Mvc;
using ErrorOr;
using MediatR;
using Localemgmt.Application.Users.Commands.Register;
using Localemgmt.Application.Users.Queries.Login;
using Localemgmt.Application.Users.Commons;
using Localemgmt.Contracts.Users;
using Mapster;


namespace Localemgmt.Api.Controllers;

[ApiController]
[Route("user")]
public class UsersController : ControllerBase
{
	ISender _mediator;


	public UsersController(ISender mediator)
	{
		_mediator = mediator;
	}


	[HttpPost]
	[Route("register")]
	public async Task<ActionResult<UserRegistrationResponse>> Register(UserRegistrationRequest request)
	{
		var command = request.Adapt<UserRegistrationCommand>();
		ErrorOr<bool> serviceResult = await _mediator.Send(command);
		return serviceResult.Match(
		  value => Ok(new UserRegistrationResponse(value)),
		  exp => Problem(exp.First().Description)
		);
	}


	[HttpGet]
	[Route("info/{email}")]
	public async Task<ActionResult<UserInfo>> GetUserInfo([FromRoute] string email)
	{
		var query = new UserInfoQuery(email);

		var serviceResult = await _mediator.Send(query);
		return serviceResult.Match<ActionResult<UserInfo>>(
		  value => Ok(value.Adapt<UserInfoResponse>()),
		  exp =>
		  {
			  var e = exp.First();
			  if (e.Code == Localemgmt.Domain.Errors.Users.UserNotFound.Code)
			  {
				  return Problem(e.Description, null, StatusCodes.Status404NotFound);
			  }

			  return Problem(e.Description);
		  }
		);
	}
}
