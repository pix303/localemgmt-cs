using ErrorOr;
using Localemgmt.Domain;

public interface IUsersCommandService
{
	ErrorOr<bool> RegisterUser(User user);
}
