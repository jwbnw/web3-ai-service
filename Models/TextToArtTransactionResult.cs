using System.Text.Json.Serialization;

namespace Web3Ai.Service.Models;

public class TextToArtTransactionResult
{
    public List<TextToArtImage>? StablityTextToArtImages;
}

public class TextToArtImage
{
    public string? Base64 { get; set; }

    public string? FinshReason { get; set; }

    public int Seed {get; set;}
}