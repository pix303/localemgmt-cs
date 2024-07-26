namespace Localemgmt.Contracts.Authentication;

public record RegistrationRequest
(
	string Firstname,
	string Lastname,
	string Email,
	string Password
);
