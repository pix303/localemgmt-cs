using Localemgmt.Application.Services.Users;
using Localemgmt.Contracts.Users;
using Microsoft.AspNetCore.Mvc;
using ErrorOr;

namespace Localemgmt.Api.Controllers;

[ApiController]
[Route("user")]
public class UsersController : ControllerBase
{
    IUsersService _service;
    public UsersController(IUsersService service)
    {
        _service = service;
    }


    [HttpPost]
    [Route("register")]
    public IActionResult Register(UserRegistrationRequest request)
    {
        ErrorOr<bool> serviceResult = _service.RegisterUser(new(request.Firstname, request.Lastname, request.Email, request.Role));
        return serviceResult.Match(
          value => Ok(value),
          exp => Problem(exp.First().Description)
        );
    }


    [HttpGet]
    [Route("info/{email}")]
    public ActionResult<UserInfo> GetUserInfo([FromRoute] string email)
    {
        var serviceResult = _service.GetUserInfo(email);
        return serviceResult.Match<ActionResult<UserInfo>>(
          value => Ok(value),
          exp => Problem(exp.First().Description)
        );
    }
}
