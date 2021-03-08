using SyncEngine.Core;
using SyncEngine.Core.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SyncEngine.Api.Data
{
    public class TransactionRepository : Repository, ITransactionRepository
    {
        public TransactionRepository(IOptions<ConnectionStrings> connectionStrings) : base(connectionStrings) 
        { }

        public async Task<IEnumerable<Transaction>> GetTransactions()
        {
            var sql = @"SELECT TransactionId AS Id,
                            ClientId AS ClientId,
                            [FileName] AS [FileName],
                            SafeFileName AS SafeFileName,
                            Complete AS Complete
                        FROM [Transaction];";
                        
            return QueryAsync<Transaction>(sql, null);
        }
    }
}
