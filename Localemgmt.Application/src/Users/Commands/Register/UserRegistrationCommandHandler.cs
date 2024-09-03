using ErrorOr;
using Localemgmt.Application.Users.Commons;
using MediatR;
using Mapster;
using Localemgmt.Domain;

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
		await Task.CompletedTask;

		var existingUser = _repository.GetUserInfoByEmail(request.Email);
		if (existingUser is not null)
		{
			return Localemgmt.Domain.Errors.Users.DuplicateEmail;
		}
		var user = request.Adapt<User>();
		var result = _repository.AddUserInfo(user);
		return result;
	}
}
