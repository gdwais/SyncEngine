
namespace SyncEngine.Domain
{
    public class Context
    {
        public string RecordId { get; set; }
        public string ClientId { get; set; }
        public string UniqueField { get; set; }
        public string UniqueId { get; set; }
        public string RawData { get; set; }
        public Record Record { get; set; }

    }
}