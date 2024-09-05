using System.ComponentModel.DataAnnotations;


namespace Localemgmt.Contracts.Users;

public record UserRequest
(
	[MinLength(6)]
	string Email
);
