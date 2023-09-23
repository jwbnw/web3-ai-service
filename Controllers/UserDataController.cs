using Microsoft.AspNetCore.Mvc;

namespace Web3Ai.Service.Controllers;

//TODO: Add a controller exception filter or enter a ticket to do so.
[ApiController]
[Route("[controller]")]
public class UserDataController : ControllerBase
{
    public UserDataController(ILogger<UserDataController> logger)
    {
        _logger = logger;
    }

    [HttpPost]
    [Route("CreateAccount")]
    public ActionResult<CreateAccountResponse> CreateAcount()
    {
        throw new NotImplementedException();
    }

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
