namespace Localemgmt.Application.Service.Authentication;


public record AuthenticationResult(
	Guid Id,
	string Firstname,
	string Lastname,
	string Email,
	string Token
);

public record RegistrationPayload(
	string Firstname,
	string Lastname,
	string Email,
	string Password
);

public record LoginPayload(
	string Email,
	string Password
);

public interface IAuthenticationService
{
	AuthenticationResult Register(RegistrationPayload payload);
	AuthenticationResult Login(LoginPayload payload);
}
