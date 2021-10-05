using SyncEngine.Domain;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace SyncEngine.Managers
{
    public interface IFileManager 
    {
        Task WriteFile(byte[] streamedFileContent, string fileFolder, string trustedFileNameForDisplay, string trustedFileNameForFileStoragte);
        IEnumerable<Record> GetRecordsFromFile(string fileFolder, string safeFileName);
    }
}