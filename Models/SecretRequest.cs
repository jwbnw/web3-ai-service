namespace Web3Ai.Service;

public class SecretRequest
{
    public required string PublickKey { get; set; }
    
    public required string SignedHash {get; set;}
}