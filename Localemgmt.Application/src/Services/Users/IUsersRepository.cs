using Localemgmt.Domain;

namespace Localemgmt.Application.Services.Users
{
    public interface IUsersRepository
    {
        UserInfo? GetUserInfoByEmail(string email);
        bool AddUserInfo(User user);
    }
}
