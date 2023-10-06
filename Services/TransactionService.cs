
using Web3Ai.Service.Models;
using Web3Ai.Service.Data.Entities;
using Web3Ai.Service.Data.Context;
using Web3Ai.Service.Authorization;

namespace Web3Ai.Service.Services;

public interface ITransactionService
{
    Task<UnsignedTransaction> CreateTransaction();
}

public class TransactionService : ITransactionService
{
    async Task<UnsignedTransaction> ITransactionService.CreateTransaction()
    {
        var unsignedTransaction = new UnsignedTransaction()
        {
            Tx = ""
        };

        return await Task.FromResult<UnsignedTransaction>(unsignedTransaction);
    }
}

   
