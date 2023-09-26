namespace Web3Ai.Service.Models;

public class InitChallengeResponse
{
    /// <summary>
    /// Hash to be signed by the client accepting 
    /// the challenge
    /// </summary>
    public required string Hash { get; set; }

    /// <summary>
    /// Id to POST with the signed Hash when
    /// verifying completion of challenge.
    /// </summary>
    public required string Id {get; set;}
}