using SyncEngine.Core;
using SyncEngine.Core.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SyncEngine.Data
{
    public class RecordRepository : Repository, IRecordRepository
    {
        public RecordRepository(IOptions<ConnectionStrings> connectionStrings) : base(connectionStrings) 
        { }

        public Task<string> CreateRecord(string clientId, string rawData)
        {
            var sql = @"INSERT INTO [Record]
                            (ClientId, RawData, ProcessingStatusId)
                            OUTPUT Inserted.RecordId
                        VALUES
                            (@clientId, @rawData, @status);";
            
            return QuerySingleAsync<string>(sql, new 
            {
                clientId, 
                rawData,
                ProcessingStatus.Pending
            });
        }

        public Task UpdateProcessingStatus(string recordId, ProcessingStatus status)
        {
            var sql = @"UPDATE [Record] SET ProcessingStatus = @status WHERE RecordId = @recordId;";
            
            return ExecuteAsync(sql, new 
            {
                recordId,
                status
            });
        }

    }

}
