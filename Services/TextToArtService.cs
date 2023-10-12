
using Web3Ai.Service.Models;
using Web3Ai.Service.Models.TextToArt;
using System.Text.Json;
using static System.Net.Mime.MediaTypeNames;
using System.Text;

using System.Net.Http.Headers;

namespace Web3Ai.Service.Services;

public interface ITextToArtService
{
    Task<TextToArtTransactionResult> GenerateArt(ValidatedTextToArtTranscationRequest transcationRequest);
}

public class TextToArtService : ITextToArtService
{

    public TextToArtService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    async Task<TextToArtTransactionResult> ITextToArtService.GenerateArt(ValidatedTextToArtTranscationRequest transcationRequest)
    {
        // esstablish http client
        var stablityHttpClient = _httpClientFactory.CreateClient("StablityClient");

        // make text to art request.
        var postData = new StringContent(
            JsonSerializer.Serialize(transcationRequest),
            Encoding.UTF8,
            Application.Json
        );

        stablityHttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("ValueHere");

        //try for debugging  top level will handle
 
            var response = await stablityHttpClient.PostAsync("https://api.stablity.ai/v1/generation/stable-diffusion-x1-1024-v1-0/text-to-image", postData);
            var rawContent = await response.Content.ReadAsStringAsync();

            var stablityTextToArtResponse = JsonSerializer.Deserialize<StablityTextToArtResponse>(rawContent);

            var finalList = new TextToArtTransactionResult();

            if(stablityTextToArtResponse?.StablityTextToArtImages == null) throw new ArgumentNullException("StablityTextToArtImages");


            foreach(var value in stablityTextToArtResponse.StablityTextToArtImages)
            {
                var finalImage = new TextToArtImage()
                {
                    Base64 = value.Base64,
                    FinshReason = value.FinshReason,
                    Seed = value.Seed
                };

                finalList?.StablityTextToArtImages?.Add(finalImage);
            }           
            
            if (finalList == null) throw new ArgumentNullException("finalList"); // clean this up
            
            return finalList; 
    }
    private readonly IHttpClientFactory _httpClientFactory;
}


