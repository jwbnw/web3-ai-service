
using Web3Ai.Service.Models;
using Web3Ai.Service.Data.Entities;
using Web3Ai.Service.Data.Context;
using Web3Ai.Service.Authorization;

namespace Web3Ai.Service.Services;

//Make Explicit Please..
public interface IUserDataService
{
    AuthenticateResponse? Authenticate(SecretRequest secretRequest);

    CreateAccountResponse? CreateAccount(SecretRequest secretRequest);

    bool HasAccount(string publicKey);

    User? GetById(Guid id);
}

public class UserDataService : IUserDataService
{

    public UserDataService(UserContext context, IJwtUtils jwtUtils)
    {
        _context = context;
        _jwtUtils = jwtUtils;
    }

    public AuthenticateResponse? Authenticate(SecretRequest secretRequest)
    {
        var user = _context.Users.SingleOrDefault(user => user.Wallet == secretRequest.PublicKey);

        if (user == null) return null;

        var token = _jwtUtils.GenerateJwtToken(user);

        return new AuthenticateResponse(user, token);
    }

    public CreateAccountResponse? CreateAccount(SecretRequest secretRequest)
    {
        var exists = _context.Users.SingleOrDefault(user => user.Wallet == secretRequest.PublicKey);

        if (exists != null) throw new ArgumentException("User already exists..");

        var user = new User()
        {
            UserId = Guid.NewGuid(),
            Wallet = secretRequest.PublicKey,
            CreateDate = DateTime.Now,
            LastUpdated = DateTime.Now
        };

        _context.Users.Add(user);
        _context.SaveChanges();

        var token = _jwtUtils.GenerateJwtToken(user);

        return new CreateAccountResponse(user, token);
    }

    public User? GetById(Guid userId)
    {
        return _context.Users.Find(userId);
    }

    public bool HasAccount(string publicKey)
    {
        return _context.Users.Any(user => user.Wallet == publicKey);
    }

    private readonly IJwtUtils _jwtUtils;
    private readonly UserContext _context;
}