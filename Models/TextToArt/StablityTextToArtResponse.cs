using System.Text.Json;
using System.Text.Json.Serialization;

namespace Web3Ai.Service.Models.TextToArt;

    public class Artifact
    {
        [JsonPropertyName("base64")]
        public string? Base64 { get; set; }

        [JsonPropertyName("seed")]
        public long Seed { get; set; }

        [JsonPropertyName("finishReason")]
        public string? FinishReason { get; set; }
    }


    public class StablityTextToArtResponse
    {
        [JsonPropertyName("artifacts")]
        public List<Artifact>? Artifacts { get; set; }
    }