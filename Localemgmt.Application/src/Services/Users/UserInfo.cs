using Localemgmt.Domain;

namespace Localemgmt.Application.Services.Users;

public record UserInfo(
	Guid Id,
	User User
);
