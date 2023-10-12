using System.Text.Json.Serialization;

namespace Web3Ai.Service.Models.TextToArt;

public class StablityTextToArtResponse
{
    //[JsonPropertyName("")]
    public StablityTextToArtImage[]? StablityTextToArtImages;
}

public class StablityTextToArtImage
{
    [JsonPropertyName("base64")]
    public string? Base64 { get; set; }

    [JsonPropertyName("finshReason")]
    public string? FinshReason { get; set; }

    [JsonPropertyName("seed")]
    public int Seed {get; set;}
}