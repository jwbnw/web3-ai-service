namespace Web3Ai.Service.Models;

public class ValidatedTextToArtTranscationRequest
{
  public int Steps { get; set; }
  public int Width { get; set; }
  public int Height { get; set; }
  public int Seed { get; set; }
  public int CfgScale { get; set; } // when sending to ai backend will need to serlize camelCase with underscore. probably in a different model
  public string? StylePreset { get; set; }
  public int Samples { get; set; }
  public required TextPrompt[] TextPrompts { get; set; }
}
