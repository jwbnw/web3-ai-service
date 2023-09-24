using Microsoft.AspNetCore.Mvc;

namespace Web3Ai.Service.Controllers;

//TODO: Add a controller exception filter or enter a ticket to do so.
[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    public AuthController(ILogger<UserDataController> logger)
    {
        _logger = logger;
    }

    [HttpPost]
    [Route("VerifySecret")]
    public ActionResult<GenericResponse> VerifySecret()
    {
        throw new NotImplementedException();
    }

    private readonly ILogger<UserDataController> _logger;
}
