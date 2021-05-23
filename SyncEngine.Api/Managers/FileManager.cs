using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Diagnostics;
using SyncEngine.Core.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SyncEngine.Api.Data;

namespace SyncEngine.Api.Managers
{
    public class FileManager : Disposer, IFileManager
    {
        
        private readonly ILogger<FileManager> logger;
        private readonly UploadSettings settings;
        public FileManager(ILogger<FileManager> logger, IOptions<UploadSettings> settings)
        {
            this.settings = settings.Value;
            this.logger = logger;
        }

        public Task QueueBatch(string clientId, string safeFileName)
        {
            var filePath = Path.Combine(settings.FileFolder, safeFileName);
            var rows = File.ReadLines(filePath).Skip(1);

            return Task.FromResult<int>(1);
        }

        //move this to the transformation worker
        // private static string ReplaceBetweenQuotes(string line)
        // {
        //     var indexesSet = GetIndexes(line);
        //     if (indexesSet.Count() > 0)
        //     {
        //         foreach(var indexSet in indexesSet)
        //         {
        //             line = ReplaceCommas(line, indexSet);
        //         }
        //     }
        //     return line.Replace("\"", "");
        // }

        // private static string ReplaceCommas(string str, (int, int) indexes)
        // {
        //     string subString = str.Substring(indexes.Item1 + 1, indexes.Item2 - indexes.Item1);
        //     return str.Replace(subString, subString.Replace(',', '|'));
        // }

        // private static List<(int, int)> GetIndexes(string str)
        //     => ConvertToTuple(str.Select((b, i) => b.Equals('"') ? i : -1).Where(i => i != -1).ToArray());
        
        // private static List<(int, int)> ConvertToTuple(int[] indexes)
        // {
        //     var tupleList = new List<(int, int)>();
        //     for (int i = 0; i < indexes.Length; i += 2)
        //     {
        //         tupleList.Add((indexes[i], indexes[i + 1]));
        //     }
        //     return tupleList;
        // }
    }

    

}