using Localemgmt.Domain;

namespace Localemgmt.Application.Users.Commons
{
    public interface IUsersRepository
    {
        UserInfo? GetUserInfoByEmail(string email);
        bool AddUserInfo(User user);
    }
}
