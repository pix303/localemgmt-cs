using Localemgmt.Domain;

namespace Localemgmt.Contracts.Users;

public record UserRegistrationRequest
(
	string Firstname,
	string Lastname,
	string Email,
	UserRole Role
);
