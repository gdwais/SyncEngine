using SyncEngine.Core;
using System.Threading.Tasks;

namespace SyncEngine.Data
{
    public interface IBatchRepository
    {
        Task<Batch> CreateBatch(Batch batch);
    }
}