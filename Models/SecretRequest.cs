namespace Web3Ai.Service.Models;

public class SecretRequest
{
    public required string PublicKey { get; set; }
    
    public required string SignedHash {get; set;}
}