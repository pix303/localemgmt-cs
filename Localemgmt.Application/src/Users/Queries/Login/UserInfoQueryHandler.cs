using ErrorOr;
using Localemgmt.Application.Users.Commons;
using MediatR;

namespace Localemgmt.Application.Users.Queries.Login;

public class UserInfoQueryHandler : IRequestHandler<UserInfoQuery, ErrorOr<UserInfo>>
{

	IUsersRepository _repository;

	public UserInfoQueryHandler(IUsersRepository repository)
	{
		_repository = repository;
	}


	public async Task<ErrorOr<UserInfo>> Handle(UserInfoQuery request, CancellationToken token)
	{
		var existingUser = _repository.GetUserInfoByEmail(request.Email);
		if (existingUser is not null)
		{
			return existingUser;
		}

		return Localemgmt.Domain.Errors.Users.UserNotFound;
	}
}
