using SyncEngine.Core;
using System.Threading.Tasks;

namespace SyncEngine.Api.Managers
{
    public interface IFileManager 
    {
        Task QueueBatch(string clientId, string safeFileName);
    }
}