using System.Threading.Tasks;
using SyncEngine.Core;

namespace SyncCtl.Services
{
    public interface IMessageService
    {
        bool Enqueue(Record message);
    }
}