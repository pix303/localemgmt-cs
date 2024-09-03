namespace Localemgmt.Domain;


public record User
(
	string Firstname,
	string Lastname,
	string Email,
	UserRole Role
);



public enum UserRole
{
	Admin,
	Translator,
	Reader,
}

