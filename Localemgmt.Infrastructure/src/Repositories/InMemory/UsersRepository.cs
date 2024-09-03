using Localemgmt.Application.Users.Commons;
using Localemgmt.Domain;

namespace Localemgmt.Infrastructure.Repositories.InMemory;

public class UsersRepository : IUsersRepository
{
    private List<UserInfo> _users = new List<UserInfo>();

    public bool AddUserInfo(User user)
    {
        var existingUser = GetUserInfoByEmail(user.Email);
        if (existingUser is not null)
        {
            return false;
        }
        _users.Add(new(Guid.NewGuid(), user));
        return true;
    }

    public UserInfo? GetUserInfoByEmail(string email)
    {
        return _users.FirstOrDefault(u => u.User.Email == email);
    }
}

