namespace Web3Ai.Service.Models;

public class CreateAccountRequest
{
    public required string Wallet { get; set; }

    public string? UserName { get; set; }

    public int? PhoneNumber { get; set; }

    public string? Email { get; set; }
}