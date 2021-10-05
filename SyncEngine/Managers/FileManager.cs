using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using SyncEngine.Domain;
using Microsoft.Extensions.Logging;
using System.Linq;
using SyncEngine.Messaging;
using System.Reflection;


namespace SyncEngine.Managers
{
    public class FileManager : Disposer, IFileManager
    {
        
        private readonly ILogger<FileManager> logger;
        private readonly IMessageService messageService;
        
        public FileManager(ILogger<FileManager> logger)
        {
            this.logger = logger;
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
                var record = LoadNew<Record>(fields);
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

        public void Load(object target, string[] fields)
        {
            Type targetType = target.GetType();
            PropertyInfo[] properties = targetType.GetProperties();
            
            foreach (PropertyInfo property in properties)  
            {
                if (property.CanWrite)                      
                    {
                        object[] attributes = property.GetCustomAttributes(typeof(PositionAttribute), false);
                        
                        if (attributes.Length > 0)
                        {
                            PositionAttribute positionAttr = 
                                (PositionAttribute)attributes[0];
                                        
                            int position = positionAttr.Position;

                            try
                            {
                                object data = fields[position];
                                if (positionAttr.DataTransform != string.Empty)
                                {
                                    MethodInfo method = targetType.GetMethod(positionAttr.DataTransform);
                                    data = method.Invoke(target, new object[] { data });
                                }
                                
                                if (data != null)
                                {
                                    var propertyType = property.PropertyType;
                                    var convertedData = Convert.ChangeType(data, propertyType);
                                    property.SetValue(target, convertedData, null);
                                }
                            }
                            catch (Exception ex)
                            {
                                if (ex.Message != NullObjectError)
                                    logger.LogError("ERROR LOADING FILE :: " + ex.Message.ToString());
                            }        
                        }
                        
                }    
            }
        }   
        
        private static string NullObjectError = "Null object cannot be converted to a value type.";

        public X LoadNew<X>(string[] fields)
        {   
            X tempObj = (X) Activator.CreateInstance(typeof(X));
            Load(tempObj, fields);
            return tempObj;
        }
    }

    

}