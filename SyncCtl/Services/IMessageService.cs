using System.Threading.Tasks;

namespace SyncCtl.Services
{
    public interface IMessageService
    {
        bool Enqueue(string message);
    }
}