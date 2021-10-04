using SyncEngine.Core;
using SyncEngine.Core.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SyncEngine.Data
{
    public class BatchRepository : Repository, IBatchRepository
    {
        public BatchRepository(IOptions<ConnectionStrings> connectionStrings) : base(connectionStrings) 
        { }

        public async Task<Batch> CreateBatch(Batch batch)
        {
            var sql = @"INSERT INTO [Batch]
                            (ClientId, FileName, SafeFileName, BatchStageId)
                        OUTPUT
                        CAST(inserted.BatchId
                        as
                        varchar(40))
                        VALUES
                            (@clientId, @fileName, @safeFileName, @stage);";
            
            var id = await QuerySingleAsync<string>(sql, new 
            {
                clientId = batch.ClientId,
                fileName = batch.FileName,
                safeFileName = batch.SafeFileName,
                stage = batch.Stage
            });

            var newBatch = batch;
            newBatch.Id = id;
            return newBatch;
        }
    }
}
