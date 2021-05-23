using SyncEngine.Core;
using SyncEngine.Core.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SyncEngine.Api.Data
{
    public class BatchRepository : Repository, IBatchRepository
    {
        public BatchRepository(IOptions<ConnectionStrings> connectionStrings) : base(connectionStrings) 
        { }

        public Task CreateBatch(Batch batch)
        {
            var sql = @"INSERT INTO [Batch]
                            (ClientId, FileName, SafeFileName, BatchStageId)
                        VALUES
                            (@clientId, @fileName, @safeFileName, @stage);";
            
            return ExecuteAsync(sql, new 
            {
                clientId = batch.ClientId,
                fileName = batch.FileName,
                safeFileName = batch.SafeFileName,
                stage = batch.Stage
            });
        }
    }
}
