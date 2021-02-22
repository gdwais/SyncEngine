using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Net.Http.Headers;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;


namespace SyncEngine.Api.Helpers
{
    public static class FileHelper
    {
        public static async Task<byte[]> ProcessStreamedFile(MultipartSection section, ContentDispositionHeaderValue contentDisposition, ModelStateDictionary modelState, string[] allowedExtensions, long sizeLimit)
        {
            try
            {
                using var memStream = new MemoryStream();
                await section.Body.CopyToAsync(memStream);
                if (memStream.Length == 0)
                    modelState.AddModelError("File", "The file is emtpy");
                else if (memStream.Length > sizeLimit)
                    modelState.AddModelError("File", $"The file is too large.  Greater than " + sizeLimit / 1048576 + " MB. ");
                else  if (!IsValidFileExtension(contentDisposition.FileName.Value, memStream, allowedExtensions))
                    modelState.AddModelError("File", "The file extension is not allowed.");
                else 
                {
                    return memStream.ToArray();
                }
            } 
            catch (Exception ex)
            {
                modelState.AddModelError("File", "something broke in the upload. ERROR: " + ex.HResult);
            }
            return new byte[0];
        }

        public static bool IsEmpty(Stream stream) => stream == null || stream.Length == 0;
        
        public static bool IsValidFileExtension(string fileName, MemoryStream memoryStream, string[] allowedExtensions)
        {
            var extension = NormalizeExtension(Path.GetExtension(fileName));
            return !(string.IsNullOrEmpty(extension) || !allowedExtensions.Contains(extension));
        }

        private static string NormalizeExtension(string extension)
        {
            if (extension == null)
                return "";
            
            extension = extension.ToLowerInvariant();
            extension = extension.StartsWith('.') ? extension : $".{extension}";
            return extension.Trim();
        }
    }
}