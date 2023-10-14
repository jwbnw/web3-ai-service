
using Web3Ai.Service.Models;
using Web3Ai.Service.Utils;
using Web3Ai.Service.Models.TextToArt;
using System.Text.Json;
using static System.Net.Mime.MediaTypeNames;
using System.Text;
using System.Net.Http.Headers;
using Microsoft.Extensions.Options;

namespace Web3Ai.Service.Services;

public interface ITextToArtService
{
    Task<TextToArtTransactionResult> GenerateArt(ValidatedTextToArtTranscationRequest transcationRequest);
}

public class TextToArtService : ITextToArtService
{
    public TextToArtService(IHttpClientFactory httpClientFactory, IOptions<AppSettings> appSettings)
    {
        _httpClientFactory = httpClientFactory;
        _appSettings = appSettings.Value;
    }

    async Task<TextToArtTransactionResult> ITextToArtService.GenerateArt(ValidatedTextToArtTranscationRequest transcationRequest)
    {
       if (_appSettings.StablityApiKey == null) throw new ArgumentNullException("Api Key should not be null;");
       
        var stablityHttpClient = _httpClientFactory.CreateClient("StablityClient");
        stablityHttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(_appSettings.StablityApiKey);

        var postData = new StringContent(
            JsonSerializer.Serialize(transcationRequest),
            Encoding.UTF8,
            Application.Json
        );

        var response = await stablityHttpClient.PostAsync("https://api.stability.ai/v1/generation/stable-diffusion-xl-1024-v1-0/text-to-image", postData);
        var rawContent = await response.Content.ReadAsStringAsync();

        var stablityTextToArtResponse = JsonSerializer.Deserialize<StablityTextToArtResponse>(rawContent);

        var finalList = new TextToArtTransactionResult
        {
            StablityTextToArtImages = new List<TextToArtImage>()
        };

        if (stablityTextToArtResponse?.Artifacts == null) throw new ArgumentNullException("StablityTextToArtImages");

        foreach (var artifact in stablityTextToArtResponse.Artifacts)
        {
            if (artifact == null) throw new ArgumentNullException("artifact");

            var finalImage = new TextToArtImage()
            {
                Base64 = artifact.Base64,
                FinishReason = artifact.FinishReason,
                Seed = artifact.Seed
            };

            finalList.StablityTextToArtImages.Add(finalImage);
        }

        if (finalList == null) throw new ArgumentNullException("finalList"); // clean this up

        return finalList;
    }
    private readonly IHttpClientFactory _httpClientFactory;

    private readonly AppSettings _appSettings;
}
