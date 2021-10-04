using SyncEngine.Core;
using SyncEngine.Data;

namespace SyncEngine.Worker
{
    public class BatchProcessor
    {

        private readonly IMessageService messageService;
        private readonly IFileManager fileManager;
        public BatchProcessor(IMessageService messageService, IFileManager fileManager) 
        {
            this.messageService = messageService;
            this.fileManager = fileManager;
        }

        public bool ProcessBatch(Batch batch)
        {
            var records = fileManager.GetRecordsFromFile("../files", batch.SafeFileName);
            foreach(var record in records)
            {
                messageService.Enqueue<Record>(record);   
            }
            return true;
        }

    }
}