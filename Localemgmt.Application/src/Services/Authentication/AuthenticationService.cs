namespace Localemgmt.Application.Service.Authentication;

public class AuthenticationService : IAuthenticationService
{
	public AuthenticationResult Login(LoginPayload payload)
	{
		return new AuthenticationResult(
			Guid.NewGuid(),
			"to implement",
			"to implement",
			payload.Email,
			"to implement"
		);

	}

	public AuthenticationResult Register(RegistrationPayload payload)
	{
		return new AuthenticationResult(
			Guid.NewGuid(),
			payload.Firstname,
			payload.Lastname,
			payload.Email,
			"to implement"
		);
	}
}
