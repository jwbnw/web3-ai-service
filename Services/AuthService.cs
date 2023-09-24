using System.Text;
using TweetNaclSharp;

namespace Web3Ai.Service;

internal interface IAuthService
{
    /// <summary>
    /// TODO:
    /// The challenge should begin
    /// with the backend to be the 
    /// most secure. It can originate 
    /// from the UI for the time being.
    /// </summary>
    /// <returns></returns>
    string InitChallenge();

    /// <summary>
    /// Solve a cryptographic challenge
    /// to ensure that a cypher provided
    /// by the client was created by the 
    /// client.
    /// </summary>
    /// <returns></returns>
    bool SolveChallenge(string pubkey, string hash, string signedHash);
}

internal class AuthService : IAuthService 
{
    internal AuthService(ILogger<AuthService> logger, UTF8Encoding encoder)
    {
        _encoder = encoder;
        _logger = logger;
    }

    string IAuthService.InitChallenge()
    {
        throw new NotImplementedException();
    }

    // Note: We're doing ed25519 signature verication. I think BouncyCastle
    // can do this but I'm going to bring in TweetNaclSharp for the time being
    // I'll have a low priorty tech debt item to bring this in-house or look
    // into a better solution. No time to roll my own. Also I don't see why
    // I would need to do any special encoding here such as b58 but who knows..
    bool IAuthService.SolveChallenge(string pubkey, string hash, string signedHash)
    {   
        if (pubkey == null || hash == null || signedHash == null)
        {
            throw new ArgumentNullException("pubkey, hash, or signed hash");
        }

        var byteHash = _encoder.GetBytes(hash);
        var byteSignedHash = _encoder.GetBytes(signedHash);
        var bytePubKey = _encoder.GetBytes(pubkey);

        return Nacl.SignDetachedVerify(byteHash, byteSignedHash, bytePubKey);
    }

    private readonly UTF8Encoding _encoder;
    private readonly ILogger<AuthService> _logger;
}