using Localemgmt.Application.Service.Authentication;
using Localemgmt.Contracts.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace Localemgmt.Api.Controllers;

[ApiController]
[Route("auth")]
public class AuthenticationController : ControllerBase
{
	IAuthenticationService _service;
	public AuthenticationController(IAuthenticationService service)
	{
		_service = service;
	}


	[HttpPost]
	[Route("register")]
	public async Task<ActionResult<AuthenticationResponse>> Register(RegistrationRequest request)
	{
		var serviceResult = _service.Register(new RegistrationPayload
		(
			request.Firstname,
			request.Lastname,
			request.Email,
			request.Password
			)
		);

		return Ok(mapServiceResponse(serviceResult));
	}


	[HttpPost]
	[Route("login")]
	public async Task<ActionResult<AuthenticationResponse>> Login(LoginRequest request)
	{

		var serviceResult = _service.Login(new LoginPayload
		(
			request.Email,
			request.Password
			)
		);


		return Ok(mapServiceResponse(serviceResult));
	}


	private AuthenticationResponse mapServiceResponse(AuthenticationResult data)
	{
		AuthenticationResponse result = new(
			data.Id,
			data.Firstname,
			data.Lastname,
			data.Email,
			data.Token
		);

		return result;
	}
}
