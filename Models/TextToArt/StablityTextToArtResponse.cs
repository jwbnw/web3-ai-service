using System.Text.Json.Serialization;

namespace Web3Ai.Service.Models.TextToArt;

    public class Artifact
    {
        public string base64 { get; set; }
        public long seed { get; set; }
        public string finishReason { get; set; }
    }

    public class StablityTextToArtResponse
    {
        public List<Artifact> artifacts { get; set; }
    }