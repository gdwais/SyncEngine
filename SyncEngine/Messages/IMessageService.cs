using System.Threading.Tasks;
using SyncEngine.Core;

namespace SyncEngine.Core.Messages
{
    public interface IMessageService
    {
        bool Enqueue<T>(T message);
    }
}