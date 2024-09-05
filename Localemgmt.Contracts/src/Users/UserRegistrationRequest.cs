using Localemgmt.Domain;
using System.ComponentModel.DataAnnotations;

namespace Localemgmt.Contracts.Users;

public record UserRegistrationRequest
(

	[MinLength(3), MaxLength(32)]
	string Firstname,

	[MinLength(3), MaxLength(32)]
	string Lastname,

	[MinLength(6), MaxLength(128), EmailAddress]
	string Email,

	[Required]
	UserRole Role
);
