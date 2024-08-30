using ErrorOr;
using Localemgmt.Domain;
using MediatR;

namespace Localemgmt.Application.Users.Commands.Register;

public record UserRegistrationCommand(
    string Firstname,
    string Lastname,
    string Email,
    UserRole Role
) : IRequest<ErrorOr<bool>>;
