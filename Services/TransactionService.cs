
using Web3Ai.Service.Models;
using Web3Ai.Service.Models.Rpc;
using System.Text.Json;
using static System.Net.Mime.MediaTypeNames;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace Web3Ai.Service.Services;

public interface ITransactionService
{
    Task<ValidatedTextToArtTranscationRequest> VerifyTransaction(TextToArtTranscationRequest transcationRequest);
}

public class TransactionService : ITransactionService
{
    public TransactionService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }
    async Task<ValidatedTextToArtTranscationRequest> ITransactionService.VerifyTransaction(TextToArtTranscationRequest transcationRequest)
    {
        if (transcationRequest.TransactionRequest.Signature == null) throw new ArgumentNullException();

        var parsedTransactionValidationResult = await GetTransactionValidationResult(transcationRequest.TransactionRequest.Signature);
        return ValidationTransactionResult(parsedTransactionValidationResult, transcationRequest);
    }

    private async Task<ParsedTransactionValidationResult> GetTransactionValidationResult(string txid)
    {
        // Do a better job with http client, this + the using is hacky.. 
        var txClient = _httpClientFactory.CreateClient("GetTransaction");
        var requestParams = new string[2];

        requestParams[0] = txid;
        requestParams[1] = "jsonParsed";

        var rpcRequest = new SolanaRpcRequest()
        {
            JsonRpc = "2.0",
            Id = 1,
            Method = "getTransaction",
            Params = requestParams
        };

        var txData = new StringContent(
         JsonSerializer.Serialize(rpcRequest),
         Encoding.UTF8,
         Application.Json
        );



            // don't do this live. just for testing/hackathon rush
            var responseMsg = await txClient.PostAsync("https://api.devnet.solana.com", txData);

            // same thing as above here..
            var rawRpcResponse = await responseMsg.Content.ReadAsStringAsync();

            var solanaTransactionResponse = JsonSerializer.Deserialize<SolanaGetTransactionResponse>(rawRpcResponse) ?? throw new ArgumentException("Solana Transaction Resposne Was Null");
           
           if(solanaTransactionResponse.result == null) throw new InvalidOperationException("Unable to communicate with Solana Node RPC");
           
            var finalInstructions = solanaTransactionResponse.result.transaction.message.instructions[2]; // Confrim it's always going to be the last instruction
            var blockTime = solanaTransactionResponse.result.blockTime;

            var transactionValidationResult = new ParsedTransactionValidationResult()
            {
                Transaction = txid,
                Blocktime = blockTime,
                OriginWallet = finalInstructions.parsed.info.source,
                DestinationWallet = finalInstructions.parsed.info.destination,
                AmmountLamports = finalInstructions.parsed.info.lamports,
                TransactionType = finalInstructions.parsed.type
            };

            return transactionValidationResult;
        
    }

    private ValidatedTextToArtTranscationRequest ValidationTransactionResult(ParsedTransactionValidationResult parsedTransactionValidationResult, TextToArtTranscationRequest transcationRequest)
    {
        //Sender
        if (parsedTransactionValidationResult.OriginWallet != transcationRequest.TransactionRequest.PayerKey)
        {
            throw new ValidationException("Transaction Invalid. Code: {cs code}");
        }

        //Reciever
        if (parsedTransactionValidationResult.DestinationWallet != "9GXxoq5MFKe3Zwh36EKJrRNMCauf3j83iUWHp6qKc4HG")
        {
            throw new ValidationException("Transaction Invalid. Code: {cs code}");
        }

        //Ammount (make ~10c for demo if no time to flush out)
        if (parsedTransactionValidationResult.AmmountLamports < 5000000)
        {
            throw new ValidationException("Transaction Invalid. Code: {cs code}");
        }

        //Type
        if (parsedTransactionValidationResult.TransactionType != "transfer")
        {
            throw new ValidationException("Transaction Invalid. Code: {cs code}");
        }

        //Time (transaction must be less than 30 Seconds old - when we go live we'll have a self service tx redemption tool.)
        if (!isValidTimeStamp(parsedTransactionValidationResult.Blocktime))
        {
            throw new ValidationException("Transaction Invalid. Code: {cs code}");
        }

        // TODO:
        // implement a cache and ensure that the signature is not in the cache, if so add it so 
        // it cannot be re-used. 

        return new ValidatedTextToArtTranscationRequest()
        {
            Steps = transcationRequest.Steps,
            Width = 1024,
            Height = 1024,
            Seed = 0,
            CfgScale = transcationRequest.CfgScale,
            Samples = 1,
            StylePreset = transcationRequest.StylePreset,
            TextPrompts = transcationRequest.TextPrompts,
        };
    }

    //TODO: Needs testing.
    private bool isValidTimeStamp(int blocktime)
    {
        var blockTimeDateTime = DateTimeOffset.FromUnixTimeSeconds(blocktime);

        var deltaSeconds = (DateTime.Now.ToUniversalTime() - blockTimeDateTime).TotalSeconds;


        if (deltaSeconds <= 60)
            return true;

        return false;

    }

    // This needs to get wrapped into some HttpProxy class but this works for now..
    private readonly IHttpClientFactory _httpClientFactory;

}


