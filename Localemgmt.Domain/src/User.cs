namespace Localemgmt.Domain;

public class User
{
    public string Firstname { get; set; } = null!;
    public string Lastname { get; set; } = null!;
    public UserRole Role { get; set; } = UserRole.Reader;
}


public enum UserRole
{
    Admin,
    Translator,
    Reader,
}

