using SyncEngine.Core;
using System.Threading.Tasks;

namespace SyncEngine.Data
{
    public interface IRecordRepository
    {
        Task<string> CreateRecord(string clientId, string rowData);
    }
}