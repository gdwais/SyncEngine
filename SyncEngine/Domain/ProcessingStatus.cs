
namespace SyncEngine.Domain
{
    public enum ProcessingStatus
    {
        Pending = 1,
        Transforming,
        Transformed,
        Validating,
        InValid,
        Validated,
        Uploading,
        Uploaded,
        Processed
    }
}