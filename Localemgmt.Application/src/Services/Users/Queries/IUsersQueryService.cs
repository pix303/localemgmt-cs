using ErrorOr;
using Localemgmt.Application.Services.Users;

public interface IUsersQueryService
{
	ErrorOr<UserInfo> GetUserInfo(string email);
}

