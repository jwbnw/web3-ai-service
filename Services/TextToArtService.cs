
using Web3Ai.Service.Models;
using Web3Ai.Service.Authorization;
using Microsoft.Extensions.Caching.Memory;
using Web3Ai.Service.Utils;
using System.Net.Http;
using System.Text.Json;
using static System.Net.Mime.MediaTypeNames;
using System.Text;
using System.Diagnostics;
using System.ComponentModel.DataAnnotations;

namespace Web3Ai.Service.Services;

public interface ITextToArtService
{
    Task<TextToArtTransactionResult> GenerateArt(ValidatedTextToArtTranscationRequest transcationRequest);
}

public class TextToArtService : ITextToArtService
{

    public TextToArtService()
    {
    }

    async Task<TextToArtTransactionResult> ITextToArtService.GenerateArt(ValidatedTextToArtTranscationRequest transcationRequest)
    {
        return await MockResult();
    }

    private async Task<TextToArtTransactionResult> MockResult()
    {
        return new TextToArtTransactionResult()
        {
            Tx = ""
        };
    }
}


