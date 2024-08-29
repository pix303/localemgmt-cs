using Localemgmt.Domain;
using ErrorOr;

namespace Localemgmt.Application.Services.Users;

public class UsersService : IUsersService
{

  IUsersRepository _repository;

  public UsersService(IUsersRepository repository)
  {
    _repository = repository;
  }

  public ErrorOr<UserInfo> GetUserInfo(string email)
  {

    var user = _repository.GetUserInfoByEmail(email);
    if (user is not null)
    {
      return user;
    }
    return Localemgmt.Domain.Errors.Users.UserNotFound;
  }

  public ErrorOr<bool> RegisterUser(User user)
  {
    var userAdded = _repository.AddUserInfo(user);
    if (userAdded)
    {
      return true;
    }

    return Localemgmt.Domain.Errors.Users.DuplicateEmail;

  }
}
