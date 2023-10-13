namespace Web3Ai.Service.Models;

public class TextToArtTranscationRequest
{
  public int Steps { get; set; }

  public int Width { get; set; }

  public int Height { get; set; }

  public int Seed { get; set; }

  public int CfgScale { get; set; }

  public string? StylePreset { get; set; }

  public int Samples { get; set; }

  public required TextPrompt[] TextPrompts { get; set; }

  public required TransactionDetail TransactionRequest { get; set; }
}

public class TextPrompt
{
  public required string Text { get; set; }

  public int Weight { get; set; }
}

public class TransactionDetail
{
  public string? Signature { get; set; }

  public string? PayerKey { get; set; }
}
