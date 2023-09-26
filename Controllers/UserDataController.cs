using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Web3Ai.Service.Authorization;
using Web3Ai.Service.Models;
using Web3Ai.Service.Utils;

namespace Web3Ai.Service.Controllers;

//TODO: Add a controller exception filter or enter a ticket to do so.
[ApiController]
[Authorize]
[Route("api/[controller]")]
public class UserDataController : ControllerBase
{
    public UserDataController(ILogger<UserDataController> logger)
    {
        _logger = logger;
    }

    [SwaggerOperationFilter(typeof(SwaggerHeaderFilter))]
    [HttpPost]
    [Route("UpdateAccount")]
    public ActionResult<CreateAccountResponse> UpdateAccount()
    {
        throw new NotImplementedException();
    }

    [HttpGet]
    [Route("GetUser")]
    public ActionResult<TestUser> GetUser()
    {
        throw new NotImplementedException();
    }

    [AllowAnonymous]
    [HttpGet]
    [Route("GetTestUser")]
    public ActionResult<TestUser> GetTestUser()
    {
        var testUser = new TestUser()
        {
            Wallet = "TestWallet",
            UserName = "TestUsername",
        };

        return testUser;
    }

    private readonly ILogger<UserDataController> _logger;
}
