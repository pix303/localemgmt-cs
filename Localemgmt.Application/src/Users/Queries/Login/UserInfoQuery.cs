using ErrorOr;
using Localemgmt.Application.Users.Commons;
using MediatR;

namespace Localemgmt.Application.Users.Queries.Login;

public record UserInfoQuery(
	string Email
) : IRequest<ErrorOr<UserInfo>>;
