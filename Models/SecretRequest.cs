namespace Web3Ai.Service.Models;

public class SecretRequest
{
    public required string PublickKey { get; set; }
    
    public required string SignedHash {get; set;}
}