using ErrorOr;

namespace Localemgmt.Domain.Errors
{
  public static class Users
  {
    public static Error DuplicateEmail => Error.Conflict
      (
        code: "Users.DuplicateEmail",
        description: "Email already exists"
        );

    public static Error UserNotFound => Error.NotFound
      (
        code: "Users.NotFound",
        description: "User not found"
        );
  }

}
