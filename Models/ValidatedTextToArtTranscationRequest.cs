using System.Text.Json.Serialization;

namespace Web3Ai.Service.Models;

public class ValidatedTextToArtTranscationRequest
{
  public int Steps { get; set; }

  public int Width { get; set; }

  public int Height { get; set; }

  public int Seed { get; set; }

  public int CfgScale { get; set; }

  [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
  public string? StylePreset { get; set; }

  public int Samples { get; set; }

  [JsonPropertyName("text_prompts")]
  public required TextPrompt[] TextPrompts { get; set; }
}
