using SyncEngine.Core;
using System.Threading.Tasks;
using SyncEngine.Data;
using System.Collections.Generic;

namespace SyncEngine.Core
{
    public interface IFileManager 
    {
        Task WriteFile(byte[] streamedFileContent, string fileFolder, string trustedFileNameForDisplay, string trustedFileNameForFileStoragte);
        IEnumerable<Record> GetRecordsFromFile(string fileFolder, string safeFileName);
    }
}