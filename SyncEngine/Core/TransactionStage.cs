namespace SyncEngine.Core
{
    public enum TransactionStage
    {
        Created = 1,
        Processing,
        Valid, 
        Invalid,
        Complete
    }
}