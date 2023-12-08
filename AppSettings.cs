namespace Web3Ai.Service;

public class AppSettings
{
    public string? JwtSecret{ get; set; }

    public string? StablityApiKey{get; set; }

    public string? ServiceWallet { get; set; }

    public string? SolanaRpcNodeUrl { get; set; }
}