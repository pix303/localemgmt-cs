using ErrorOr;
using Localemgmt.Application.Users.Commons;
using MediatR;

namespace Localemgmt.Application.Users.Commands.Register;

public class UserRegistrationCommandHandler : IRequestHandler<UserRegistrationCommand, ErrorOr<bool>>
{
	IUsersRepository _repository;

	public UserRegistrationCommandHandler(IUsersRepository repository)
	{
		_repository = repository;
	}


	public async Task<ErrorOr<bool>> Handle(UserRegistrationCommand request, CancellationToken token)
	{
		var existingUser = _repository.GetUserInfoByEmail(request.Email);
		if (existingUser is not null)
		{
			return Localemgmt.Domain.Errors.Users.DuplicateEmail;
		}

		var result = _repository.AddUserInfo(new(request.Firstname, request.Lastname, request.Email, request.Role));
		return result;
	}
}
