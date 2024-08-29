using ErrorOr;

namespace Localemgmt.Application.Services.Users;

public class UsersQueryService : IUsersQueryService
{

    IUsersRepository _repository;

    public UsersQueryService(IUsersRepository repository)
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

}
