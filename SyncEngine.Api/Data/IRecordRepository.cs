using SyncEngine.Core;
using System.Threading.Tasks;

namespace SyncEngine.Api.Data
{
    public interface IRecordRepository
    {
        Task CreateRecord(string clientId, string rowData);
    }
}