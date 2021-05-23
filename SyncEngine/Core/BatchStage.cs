namespace SyncEngine.Core
{
    public enum BatchStage
    {
        Created = 1,
        Processing,
        Valid, 
        Invalid,
        Complete
    }
}