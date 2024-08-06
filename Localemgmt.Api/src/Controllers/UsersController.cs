using Localemgmt.Application.Services.Users;
using Localemgmt.Contracts.Users;
using Microsoft.AspNetCore.Mvc;

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
  public Task<ActionResult<bool>> Register(UserRegistrationRequest request)
  {
    var serviceResult = _service.RegisterUser(new(request.Firstname, request.Lastname, request.Email, request.Role));
    return Task.FromResult<ActionResult<bool>>(Ok(serviceResult));
  }


  [HttpGet]
  [Route("info/{email}")]
  public Task<ActionResult<UserInfo>> GetUserInfo([FromRoute] string email)
  {
    var serviceResult = _service.GetUserInfo(email); if (serviceResult is not null)
    {
      return Task.FromResult<ActionResult<UserInfo>>(Ok(serviceResult));
    }
    else
    {
      return Task.FromResult<ActionResult<UserInfo>>(NotFound($"Not found user with email: {email}"));
    }
  }
}
