
using Web3Ai.Service.Models;
using Web3Ai.Service.Models.Rpc;
using System.Text.Json;
using static System.Net.Mime.MediaTypeNames;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using Microsoft.Extensions.Options;

namespace Web3Ai.Service.Services;

public interface ITransactionService
{
    Task<ValidatedTextToArtTranscationRequest> VerifyTransaction(TextToArtTranscationRequest transcationRequest);
}

public class TransactionService : ITransactionService
{
    public TransactionService(IHttpClientFactory httpClientFactory, IOptions<AppSettings> appSettings)
    {
        _appSettings = appSettings.Value;
        _httpClientFactory = httpClientFactory;
    }

    async Task<ValidatedTextToArtTranscationRequest> ITransactionService.VerifyTransaction(TextToArtTranscationRequest transcationRequest)
    {
        if (transcationRequest.TransactionRequest.Signature == null) throw new ArgumentNullException();

        var solanaEnv = GetSolanaNetwork(transcationRequest);

        var parsedTransactionValidationResult = await GetTransactionValidationResult(transcationRequest.TransactionRequest.Signature, solanaEnv);
        return ValidationTransactionResult(parsedTransactionValidationResult, transcationRequest);
    }

    private async Task<ParsedTransactionValidationResult> GetTransactionValidationResult(string txid, SolanaEnvironments solanaEnv)
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

        var solanaNetwork = "";

        solanaNetwork = solanaEnv switch
        {
            SolanaEnvironments.Dev => SolanaDevNet,
            SolanaEnvironments.Test => SolanaTestNet,
            SolanaEnvironments.Live => SolanaLiveNet,
            _ => SolanaLiveNet,
        };

        // don't do this live. just for testing/hackathon rush
        var responseMsg = await txClient.PostAsync(solanaNetwork, txData);

        // same thing as above here..
        var rawRpcResponse = await responseMsg.Content.ReadAsStringAsync();

        var solanaTransactionResponse = JsonSerializer.Deserialize<SolanaGetTransactionResponse>(rawRpcResponse) ?? throw new ArgumentException("Solana Transaction Resposne Was Null");

        if (solanaTransactionResponse.result == null) throw new InvalidOperationException("Unable to communicate with Solana Node RPC");

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

        //Reciever (TODO: should be env var)
        if (parsedTransactionValidationResult.DestinationWallet != _appSettings.ServiceWallet)
        {
            throw new ValidationException("Transaction Invalid. Code: {DestinationWallet}");
        }

        //Ammount roughly ~8c at time of commit. TODO: Build backend price calculator based on art request and service
        if (parsedTransactionValidationResult.AmmountLamports < 3619909)
        {
            throw new ValidationException("Transaction Invalid. Code: {AmmountLamports}");
        }

        //Type
        if (parsedTransactionValidationResult.TransactionType != "transfer")
        {
            throw new ValidationException("Transaction Invalid. Code: {TransactionType}");
        }

        //Time (transaction must be less than 60 Seconds old - when we go live we'll have a self service tx redemption tool.)
        if (!isValidTimeStamp(parsedTransactionValidationResult.Blocktime))
        {
            throw new ValidationException("Transaction Invalid. Code: {Blocktime}");
        }

        //TODO: This + content validation in general can be improved
        if(transcationRequest.StylePreset == "None")
        {
            transcationRequest.StylePreset = null;
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

    private bool isValidTimeStamp(int blocktime)
    {
        var blockTimeDateTime = DateTimeOffset.FromUnixTimeSeconds(blocktime);

        var deltaSeconds = (DateTime.Now.ToUniversalTime() - blockTimeDateTime).TotalSeconds;


        if (deltaSeconds <= 60)
            return true;

        return false;
    }

    //TODO: these should be case insensitive...
    private SolanaEnvironments GetSolanaNetwork(TextToArtTranscationRequest transactionReqeust)
    {
        if(transactionReqeust.TransactionRequest.Env == "Dev")
        {
            return SolanaEnvironments.Dev;
        }
        if(transactionReqeust.TransactionRequest.Env  == "Test")
        {
            return SolanaEnvironments.Test;
        }
        if(transactionReqeust.TransactionRequest.Env  == "Live")
        {
            return SolanaEnvironments.Live;
        }
        // Should never hit this fallback - TODO: log here if in case
        return SolanaEnvironments.Live;
    }

    enum SolanaEnvironments
    {
        Dev,
        Test,
        Live
    }

    // This needs to get wrapped into some HttpProxy class but this works for now..
    private readonly IHttpClientFactory _httpClientFactory;

    private readonly AppSettings _appSettings;

    private const string SolanaDevNet = "https://api.devnet.solana.com";

    private const string SolanaTestNet = "https://api.testnet.solana.com";

    private const string SolanaLiveNet = "https://rpc.helius.xyz/?api-key=8949714d-2da1-48db-9101-a9511113e7fe";

}
