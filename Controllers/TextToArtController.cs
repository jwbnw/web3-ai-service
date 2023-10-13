using Microsoft.AspNetCore.Mvc;
using Web3Ai.Service.Authorization;
using Web3Ai.Service.Models;
using Web3Ai.Service.Services;

namespace Web3Ai.Service.Controllers;

[ApiController]
[Authorize]
[Route("api/text-to-art")]
public class TextToArtController : ServiceBaseController
{
    public TextToArtController(
        ITransactionService transactionService,
        ITextToArtService textToArtService)
    {
        _transactionService = transactionService;
        _textToArtService = textToArtService;
    }

    [AllowAnonymous]
    [HttpPost]
    [Route("create-transaction")]
    public async Task<ActionResult<TextToArtTransactionResult>> CreateTransaction(TextToArtTranscationRequest request)
    {
        return await HandleActionResultErrorsAsync(async () =>
        {
            var textToArtRequest = await _transactionService.VerifyTransaction(request);
            var result = await _textToArtService.GenerateArt(textToArtRequest);

            return result;
        });
    }

    [AllowAnonymous]
    [HttpPost]
    [Route("test-transaction")]
    public async Task<ActionResult<TextToArtTransactionResult>> Test(TextToArtTranscationRequest request)
    {
        return await HandleActionResultErrorsAsync(async () =>
        {
            var textToArtRequest = await _transactionService.VerifyTransaction(request);
            var result = await _textToArtService.GenerateArt(textToArtRequest);

            return result;
        });
    }

    private readonly ITransactionService _transactionService;
    private readonly ITextToArtService _textToArtService;
}
