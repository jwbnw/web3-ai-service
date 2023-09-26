using Web3Ai.Service.Data.Entities;

namespace Web3Ai.Service.Models;

public class CreateAccountResponse
{
    public Guid UserId { get; set; }

    public string? Wallet { get; set; }

    public string? Username { get; set; }

    public int? PhoneNumber { get; set;}

    public string? Email { get; set; }

    public string Token { get; set; }

    public CreateAccountResponse(User user, string token)
    {
        UserId = user.UserId;
        Wallet = user.Wallet;
        Email = user.Email;
        PhoneNumber = user.Phone;
        Username = user.Username;
        
        Token = token;
    }

}