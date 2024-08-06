using Localemgmt.Domain;

namespace Localemgmt.Contracts.Users;

public record UserInfoResponse
(
	Guid Id,
	string Firstname,
	string Lastname,
	UserRole Role
);
