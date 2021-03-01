using System;

namespace SyncEngine.Core
{
    public class Transaction
    {
        public string Id { get; set; } = String.Empty;
        public string ClientId { get; set; } = String.Empty;
        public string FileName { get; set; } = String.Empty;
        public string SafeFileName { get; set; } = String.Empty;
        public bool Complete { get; set; } = false;
        
    }
}