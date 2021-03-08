using SyncEngine.Core;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SyncEngine.Api.Data
{
    public interface ITransactionRepository
    {
        Task<IEnumerable<Transaction>> GetTransactions();
        Task CreateTransaction(Transaction transaction);
    }
}