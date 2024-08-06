using Localemgmt.Domain;

namespace Localemgmt.Application.Services.Users;


public record UserInfo(
    Guid Id,
    User User
);


public interface IUsersService
{
  bool RegisterUser(User user);
  UserInfo? GetUserInfo(string email);
}

