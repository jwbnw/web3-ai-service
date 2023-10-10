namespace Web3Ai.Service.Models;

public class ParsedTransactionValidationResult
{
    public required string Transaction { get; set; }

    public required int Blocktime { get; set; }

    public required string OriginWallet { get; set; }

    public required string DestinationWallet { get; set; }

    public required int AmmountLamports { get; set; }

    public required string TransactionType { get; set; }
}
