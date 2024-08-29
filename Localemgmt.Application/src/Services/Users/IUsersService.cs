using Localemgmt.Domain;
using ErrorOr;

namespace Localemgmt.Application.Services.Users;


public record UserInfo(
    Guid Id,
    User User
);


public interface IUsersService
{
  ErrorOr<bool> RegisterUser(User user);
  ErrorOr<UserInfo> GetUserInfo(string email);
}

