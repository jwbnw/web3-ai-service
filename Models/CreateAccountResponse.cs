namespace Web3Ai.Service;

public class CreateAccountResponse : GenericResponse
{
    public string? Wallet { get; set; }

    public string? UserName { get; set; }

    public int? PhoneNumber { get; set;}

    public string? Email { get; set; }
}