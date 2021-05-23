using SyncEngine.Core;
using System.Threading.Tasks;

namespace SyncEngine.Api.Data
{
    public interface IBatchRepository
    {
        Task CreateBatch(Batch batch);
    }
}