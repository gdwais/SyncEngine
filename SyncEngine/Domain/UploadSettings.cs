using System.Linq;

namespace SyncEngine.Domain
{
    public class UploadSettings
    {
        public string[] AllowedExtensions { get; set; } = new string[0];
        public long SizeLimit { get; set; } = 0;
        public string FileFolder { get; set; } = "";
    }
}