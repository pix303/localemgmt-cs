using Localemgmt.Domain;

namespace Localemgmt.Application.Services.Users;

public class UsersService : IUsersService
{
	IUsersRepository _repository;
	public UsersService(IUsersRepository repository)
	{
		_repository = repository;
	}
	public UserInfo? GetUserInfo(string email)
	{
		return _repository.GetUserInfoByEmail(email);
	}

	public bool RegisterUser(User user)
	{
		return _repository.AddUserInfo(user);
	}
}
