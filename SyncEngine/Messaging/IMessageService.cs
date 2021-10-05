using System.Threading.Tasks;
using SyncEngine.Domain;

namespace SyncEngine.Messaging
{
    public interface IMessageService
    {
        Task<bool> Enqueue<T>(T message);
    }
}