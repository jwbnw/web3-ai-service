using Microsoft.AspNetCore.Mvc;

namespace Web3Ai.Service.Controllers;

[ApiController]
[Route("[controller]")]
public class UserDataController : ControllerBase
{
    private readonly ILogger<UserDataController> _logger;

    public UserDataController(ILogger<UserDataController> logger)
    {
        _logger = logger;
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
}
