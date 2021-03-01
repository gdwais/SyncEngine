using SyncEngine.Core;
using SyncEngine.Core.Configuration;
using Microsoft.Extensions.Options;

namespace SyncEngine.Api.Data
{
    public class FileRepository : Repository, IFileRepository
    {
        public FileRepository(IOptions<ConnectionStrings> connectionStrings) : base(connectionStrings) 
        { }
    }
}
