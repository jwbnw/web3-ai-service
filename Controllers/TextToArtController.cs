using Microsoft.AspNetCore.Mvc;
using Web3Ai.Service.Authorization;
using Web3Ai.Service.Models;
using Web3Ai.Service.Services;

namespace Web3Ai.Service.Controllers;

//TODO: Add a controller exception filter or enter a ticket to do so.
[ApiController]
[Authorize]
[Route("api/text-to-art")]
public class TextToArtController : ServiceBaseController
{
    public TextToArtController(
        IAuthService authService, 
        ILogger<UserDataController> logger,
        ITransactionService transactionService)
    {
        _authService = authService;
        _logger = logger;
        _transactionService = transactionService;
    }

    [AllowAnonymous]
    [HttpPost]
    [Route("create-transaction")]
    public async Task<ActionResult<UnsignedTransaction>> CreateTransaction()
    {
        return await HandleActionResultErrorsAsync(async () =>
        {
            var transaction = await _transactionService.CreateTransaction();
            return transaction;
        });
    }

    private readonly ILogger<UserDataController> _logger;
    private readonly IAuthService _authService;
    private readonly ITransactionService _transactionService;
}
