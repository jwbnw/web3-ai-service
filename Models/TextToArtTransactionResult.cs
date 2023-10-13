using System.Text.Json.Serialization;

namespace Web3Ai.Service.Models;

public class TextToArtTransactionResult
{   
    public List<TextToArtImage> StablityTextToArtImages {get; set;}
}

public class TextToArtImage
{
    public string Base64 { get; set; }

    public string FinishReason { get; set; }

    public long Seed {get; set;}
}