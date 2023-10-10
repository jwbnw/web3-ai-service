using System.Text.Json.Serialization;

namespace Web3Ai.Service.Models.Rpc;

public class SolanaRpcRequest
{
    [JsonPropertyName("jsonrpc")]
    public required string JsonRpc { get; set; }

    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("method")]
    public required string Method {get; set;}

    [JsonPropertyName("params")]
    public required string[] Params { get; set; }
}