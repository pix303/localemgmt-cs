using Localemgmt.Application.Services.Messages;
using Localemgmt.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Localemgmt.Api.src.Controllers;

[Authorize]
[ApiController]
[Route("api/messages")]
public class MessagesController : ControllerBase
{
    private IGenericMessageService _service;
    public MessagesController(IGenericMessageService service)
    {
        _service = service;
    }


    [HttpGet]
    [Route("test")]
    public ActionResult<GenericMessage> GetTestMessage()
    {
        return Ok(_service.GetGenericMessage());
    }
}
