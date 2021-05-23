using System;

namespace SyncEngine.Core
{
    public class Batch
    {
        public string Id { get; set; } = String.Empty;
        public string ClientId { get; set; } = String.Empty;
        public string FileName { get; set; } = String.Empty;
        public string SafeFileName { get; set; } = String.Empty;
        public BatchStage Stage { get; set; } = 0;
        
    }
}