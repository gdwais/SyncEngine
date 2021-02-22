using System;
using System.IO;
using Microsoft.Net.Http.Headers;

namespace SyncEngine.Api.Helpers
{
    public static class MultipartRequestHelper
    {
        public static string GetBoundary(MediaTypeHeaderValue contentType, int lengthLimit)
        {
            var boundary = HeaderUtilities.RemoveQuotes(contentType.Boundary).Value;
            if (string.IsNullOrWhiteSpace(boundary))
                throw new InvalidDataException("No content-type boundary found");
            
            if (boundary.Length > lengthLimit)
                throw new InvalidDataException($"length limit exceeded {lengthLimit}");

            return boundary;
        }

        public static bool IsMultipartContentType(string contentType) => !string.IsNullOrEmpty(contentType) && contentType.IndexOf("multipart/", StringComparison.OrdinalIgnoreCase) >= 0;

        public static bool HasFormDataContentDisposition(ContentDispositionHeaderValue content) =>
            content != null && content.DispositionType.Equals("form-data") && string.IsNullOrEmpty(content.FileName.Value) && string.IsNullOrEmpty(content.FileNameStar.Value);

        public static bool HasFileContentDisposition(ContentDispositionHeaderValue content) => content != null && content.DispositionType.Equals("form-data") && (!string.IsNullOrEmpty(content.FileName.Value) || !string.IsNullOrEmpty(content.FileNameStar.Value));
    }
}