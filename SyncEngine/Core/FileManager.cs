using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using SyncEngine.Core.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Linq;
using SyncEngine.Data;


namespace SyncEngine.Core
{
    public class FileManager : Disposer, IFileManager
    {
        
        private readonly ILogger<FileManager> logger;
        private readonly IMessageService messageService;
        private readonly Loader loader;
        public FileManager(ILogger<FileManager> logger,  Loader loader)
        {
            this.logger = logger;
            this.loader = loader;
        }

        public async Task WriteFile(byte[] streamedFileContent, string fileFolder, string trustedFileNameForDisplay, string trustedFileNameForFileStorage)
        {
            using (var targetStream = System.IO.File.Create(Path.Combine(fileFolder, trustedFileNameForFileStorage)))
            {
                await targetStream.WriteAsync(streamedFileContent);
                logger.LogInformation(
                    "Uploaded file '{TrustedFileNameForDisplay}' saved to " +
                    "'{TargetFilePath}' as {TrustedFileNameForFileStorage}", 
                    trustedFileNameForDisplay, fileFolder, 
                    trustedFileNameForFileStorage);
            }
        }

        public IEnumerable<Record> GetRecordsFromFile(string fileFolder, string safeFileName)
        {
            var filePath = Path.Combine(fileFolder, safeFileName);
            var records = new List<Record>();
            string[][] data = File.ReadLines(filePath).Skip(1)
                            .Select(line => ReplaceBetweenQuotes(line).Split(','))
                            .ToArray();

            foreach(var fields in data)
            {
                var record = loader.LoadNew<Record>(fields);
                record.Initialize();
                records.Add(record);
            }
            
            return records;
        }

        private static string ReplaceBetweenQuotes(string line)
        {
            var indexesSet = GetIndexes(line);
            if (indexesSet.Count() > 0)
            {
                foreach(var indexSet in indexesSet)
                {
                    line = ReplaceCommas(line, indexSet);
                }
            }
            return line.Replace("\"", "");
        }

        private static string ReplaceCommas(string str, (int, int) indexes)
        {
            string subString = str.Substring(indexes.Item1 + 1, indexes.Item2 - indexes.Item1);
            return str.Replace(subString, subString.Replace(',', '|'));
        }

        private static List<(int, int)> GetIndexes(string str)
            => ConvertToTuple(str.Select((b, i) => b.Equals('"') ? i : -1).Where(i => i != -1).ToArray());
        
        private static List<(int, int)> ConvertToTuple(int[] indexes)
        {
            var tupleList = new List<(int, int)>();
            for (int i = 0; i < indexes.Length; i += 2)
            {
                tupleList.Add((indexes[i], indexes[i + 1]));
            }
            return tupleList;
        }
    }

    

}