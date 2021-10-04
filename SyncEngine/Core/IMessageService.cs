using System.Threading.Tasks;
using SyncEngine.Core;

namespace SyncEngine.Core
{
    public interface IMessageService
    {
        Task<bool> Enqueue<T>(T message);
    }
}