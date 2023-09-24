using Microsoft.AspNetCore.Mvc;

namespace Web3Ai.Service.Controllers;

//TODO: Add a controller exception filter or enter a ticket to do so.
[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    public AuthController(IAuthService authService, ILogger<UserDataController> logger)
    {
        _authService = authService;
        _logger = logger;
    }

    [HttpGet]
    [Route("InitChallenge")]
    public ActionResult<InitChallengeResponse> InitChallenge()
    {
        throw new NotImplementedException();
    }

    [HttpPost]
    [Route("VerifySecret")]
    public ActionResult<GenericResponse> VerifySecret(SecretRequest secretRequest)
    {
        var result = _authService.SolveChallenge(secretRequest.PublickKey, TestHash, secretRequest.SignedHash);

        return new GenericResponse()
        {
            IsSuccess = result,
        };
    }

    private const string TestHash = "Hello, world";
    private readonly ILogger<UserDataController> _logger;
    private readonly IAuthService _authService;
}
