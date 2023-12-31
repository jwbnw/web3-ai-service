using Microsoft.AspNetCore.Mvc;
using Web3Ai.Service.Authorization;
using Web3Ai.Service.Models;
using Web3Ai.Service.Services;

namespace Web3Ai.Service.Controllers;

//TODO: Add a controller exception filter or enter a ticket to do so.
[ApiController]
[Authorize]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    public AuthController(
        IAuthService authService, 
        ILogger<UserDataController> logger, 
        IUserDataService userDataService)
    {
        _authService = authService;
        _logger = logger;
        _userDataService = userDataService;
    }

    [AllowAnonymous]
    [HttpGet]
    [Route("InitChallenge")]
    public ActionResult<InitChallengeResponse> InitChallenge()
    {
        throw new NotImplementedException();
    }

    [AllowAnonymous]
    [HttpPost]
    [Route("CreateAccount")]
    public ActionResult<CreateAccountResponse> CreateAccount(SecretRequest secretRequest)
    {
        var result = _authService.SolveChallenge(secretRequest.PublicKey, TestHash, secretRequest.SignedHash);
    
        if(!result) return BadRequest(new GenericResponse(){ IsSuccess = false, Error = "Challenge Failed" });
        
        var createAccountResult = _userDataService.CreateAccount(secretRequest);
        
        if(createAccountResult == null) 
        {
            return BadRequest(new GenericResponse(){ IsSuccess = false, Error = "Something went wrong" });
        }
        
        return Ok(createAccountResult);
    }

    [AllowAnonymous]
    [HttpPost]
    [Route("SignIn")]
    public ActionResult<AuthenticateResponse> SignIn(SecretRequest secretRequest)
    {
       var result = _authService.SolveChallenge(secretRequest.PublicKey, TestHash, secretRequest.SignedHash);
    
        if(!result) return BadRequest(new GenericResponse(){ IsSuccess = false, Error = "Challenge Failed" });
        
        var authResult = _userDataService.Authenticate(secretRequest);
        
        if(authResult == null) 
        {
            return BadRequest(new GenericResponse(){ IsSuccess = false, Error = "Something went wrong" });
        }
        
        return Ok(authResult);
    }
    
    [AllowAnonymous]
    [HttpPost]
    [Route("HasAccount")]
    public ActionResult<HasAccountResponse> SignIn(HasAccountRequest hasAccountRequest)
    {    
        if(string.IsNullOrWhiteSpace(hasAccountRequest.PublicKey)) return BadRequest(new GenericResponse(){ IsSuccess = false, Error = "PubKey CAnnot be empty" });
        
        var hasAccountResult = _userDataService.HasAccount(hasAccountRequest.PublicKey);
        
        var response = new HasAccountResponse()
        {
            HasAccount = hasAccountResult
        };
        
        return Ok(response);
    }

    private const string TestHash = "HashFromServer";
    private readonly ILogger<UserDataController> _logger;
    private readonly IAuthService _authService;
    private readonly IUserDataService _userDataService;
}
