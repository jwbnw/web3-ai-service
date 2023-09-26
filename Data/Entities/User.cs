using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Web3Ai.Service.Data.Entities;

public class User
{
    [Key]
    public Guid UserId { get; set; }

    public required string Wallet { get; set; }

    public string? Email { get; set; }

    public string? Username { get; set; }

    public int? Phone { get; set; }

    [JsonIgnore]
    public DateTime CreateDate {get; set;}

    [JsonIgnore]
    public DateTime LastUpdated {get; set;}
}