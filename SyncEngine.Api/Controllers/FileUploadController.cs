using SyncEngine.Api.Helpers;
using SyncEngine.Core.Configuration;
using SyncEngine.Api.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using System.Net;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using Microsoft.AspNetCore.Http.Features;

namespace SyncEngine.Api.Controllers
{
    [ApiController]
    [Route("api/file")]
    public class FileUploadController : ControllerBase
    {
        private readonly ILogger<FileUploadController> logger;
        private readonly UploadSettings settings;
        private static readonly FormOptions formOptions = new FormOptions();

        public FileUploadController(ILogger<FileUploadController> logger, IOptions<UploadSettings> settings)
        {
            this.logger = logger;
            this.settings = settings.Value;
        }

        [HttpGet]
        public async Task<int> Ping()
        {
            return 1;
        }
        
        [HttpPost]
        [DisableFormValueModelBinding]
        public async Task<IActionResult> Upload()
        {
            if (!MultipartRequestHelper.IsMultipartContentType(Request.ContentType))
            {
                ModelState.AddModelError("File", $"The request couldn't be processed (Error 1).");

                return BadRequest(ModelState);
            }

            var boundary = MultipartRequestHelper.GetBoundary(
                MediaTypeHeaderValue.Parse(Request.ContentType),
                formOptions.MultipartBoundaryLengthLimit);
            var reader = new MultipartReader(boundary, HttpContext.Request.Body);
            var section = await reader.ReadNextSectionAsync();

            while (section != null)
            {
                var hasContentDispositionHeader = 
                    ContentDispositionHeaderValue.TryParse(
                        section.ContentDisposition, out var contentDisposition);

                if (hasContentDispositionHeader)
                {
                    if (!MultipartRequestHelper.HasFileContentDisposition(contentDisposition))
                    {
                        ModelState.AddModelError("File", $"The request couldn't be processed (Error 2).");
                     
                        return BadRequest(ModelState);
                    }
                    else
                    {
                        var trustedFileNameForDisplay = WebUtility.HtmlEncode(
                                contentDisposition.FileName.Value);
                        var trustedFileNameForFileStorage = Path.GetRandomFileName();
                        var streamedFileContent = await FileHelper.ProcessStreamedFile(
                            section, contentDisposition, ModelState, 
                            settings.AllowedExtensions, settings.SizeLimit);

                        if (!ModelState.IsValid)
                        {
                            return BadRequest(ModelState);
                        }

                        using (var targetStream = System.IO.File.Create(
                            Path.Combine(settings.FileFolder, trustedFileNameForFileStorage)))
                        {
                            await targetStream.WriteAsync(streamedFileContent);

                            logger.LogInformation(
                                "Uploaded file '{TrustedFileNameForDisplay}' saved to " +
                                "'{TargetFilePath}' as {TrustedFileNameForFileStorage}", 
                                trustedFileNameForDisplay, settings.FileFolder, 
                                trustedFileNameForFileStorage);
                        }
                    }
                }

                section = await reader.ReadNextSectionAsync();
            }

            return Created(nameof(FileUploadController), null);
        }
    }
}
