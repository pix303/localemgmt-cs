namespace Localemgmt.Contracts.Authentication;

public record LoginRequest
(
	string Email,
	string Password
);
