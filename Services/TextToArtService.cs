
using Web3Ai.Service.Models;
using Web3Ai.Service.Models.TextToArt;
using System.Text.Json;
using static System.Net.Mime.MediaTypeNames;
using System.Text;

using System.Net.Http.Headers;
using System.Diagnostics;

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
        stablityHttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("");


        // make text to art request.
        var postData = new StringContent(
            JsonSerializer.Serialize(transcationRequest),
            Encoding.UTF8,
            Application.Json
        );


        //try for debugging  top level will handle
 
            var response = await stablityHttpClient.PostAsync("https://api.stability.ai/v1/generation/stable-diffusion-xl-1024-v1-0/text-to-image", postData);
            var rawContent = await response.Content.ReadAsStringAsync();

            // Debug.WriteLine(rawContent);
           // var mockResp = """{"artifacts":[{"base64":"NE1yXL9wKg/7ISKRTARJs0HeDfSPjI6sT7NscWl+qdnw4lVXKnNWnKACgGVjxtasHVu/6dDUIaRu4/oXWlPOpKqm4WyZ0IJacALEZBwC5FZUXZdty4wvPdwYkcDMh/cggZlZJRNgghuFEOzUckr9sZtq/d8oBPKuBZz/BBfrwLEG0rErEqQAfOf2hajy2Js/6k1nM/BDAMjMBDYklIQ8YJjnOakdv0F+5TVXfeZj9/zNB9/UsB/6ld94U3/MJuljGIhV40Hko0SYHGCEFYB4M9vHPDtkZyfeNbecJB7sSgRcN00/lU4Q9ZlgTa2bo1nQAAAABJRU5ErkJggg==","seed":3303980766,"finishReason":"SUCCESS"}]}""";
           // var testContent = JsonSerializer.Serialize(mockResp);

            var stablityTextToArtResponse = JsonSerializer.Deserialize<StablityTextToArtResponse>(rawContent);

            var finalList = new TextToArtTransactionResult
            {
                StablityTextToArtImages = new List<TextToArtImage>()
            };

            if (stablityTextToArtResponse?.artifacts == null) throw new ArgumentNullException("StablityTextToArtImages");


            foreach(var value in stablityTextToArtResponse.artifacts)
            {
                var finalImage = new TextToArtImage()
                {
                    Base64 = value.base64,
                    FinishReason = value.finishReason,
                    Seed = value.seed
                };

                finalList.StablityTextToArtImages.Add(finalImage);
            }           
            
            if (finalList == null) throw new ArgumentNullException("finalList"); // clean this up
            
            return finalList; 
    }
    private readonly IHttpClientFactory _httpClientFactory;
}


