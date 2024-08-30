using Localemgmt.Domain;

namespace Localemgmt.Application.Users.Commons
{
	public record UserInfo(
		Guid Id,
		User User
	);
}

