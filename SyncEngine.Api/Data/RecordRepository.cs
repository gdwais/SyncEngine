using SyncEngine.Core;
using SyncEngine.Core.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SyncEngine.Api.Data
{
    public class RecordRepository : Repository, IRecordRepository
    {
        public RecordRepository(IOptions<ConnectionStrings> connectionStrings) : base(connectionStrings) 
        { }

        public Task CreateRecord(string clientId, string rawData)
        {
            var sql = @"INSERT INTO [Record]
                            (ClientId, RawData, ProcessingStatusId)
                        VALUES
                            (@clientId, @rawData, @status);";
            
            return ExecuteAsync(sql, new 
            {
                
            });
        }
    }
}
