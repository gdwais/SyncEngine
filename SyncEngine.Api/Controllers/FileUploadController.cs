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
using SyncEngine.Domain;
using SyncEngine.Api.Helpers;
using SyncEngine.Api.Filters;
using SyncEngine.Data;
using SyncEngine.Managers;
using SyncEngine.Messaging;

namespace SyncEngine.Api.Controllers
{
    [ApiController]
    [Route("api/file")]
    public class FileUploadController : ControllerBase
    {
        private readonly ILogger<FileUploadController> logger;
        private readonly UploadSettings settings;
        private static readonly FormOptions formOptions = new FormOptions();
        private readonly IBatchRepository batches;
        private readonly IFileManager fileManager;
        private readonly IMessageService messageService;

        public FileUploadController(ILogger<FileUploadController> logger, IOptions<UploadSettings> settings, IBatchRepository batches, IFileManager fileManager, IMessageService messageService)
        {
            this.logger = logger;
            this.settings = settings.Value;
            this.batches = batches;
            this.fileManager = fileManager;
            this.messageService = messageService;
        }

        [HttpGet]
        public async Task<int> Ping()
        {
            return Result.From(100);
        }
        
        [HttpPost("{clientId}")]
        [DisableFormValueModelBinding]
        public async Task<IActionResult> Upload(string clientId)
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
                            section, 
                            contentDisposition, 
                            ModelState, 
                            settings.AllowedExtensions, 
                            settings.SizeLimit
                            );

                        if (!ModelState.IsValid)
                        {
                            return BadRequest(ModelState);
                        }
                        
                        var batch = new Batch()
                        {
                            ClientId = clientId,
                            FileName = contentDisposition.FileName.Value,
                            SafeFileName = trustedFileNameForFileStorage,
                            Stage = BatchStage.Created
                        };

                        await fileManager.WriteFile(streamedFileContent, settings.FileFolder, trustedFileNameForDisplay, trustedFileNameForFileStorage);

                        batch = await batches.CreateBatch(batch);
                        await messageService.Enqueue<Batch>(batch);
                    }
                }

                section = await reader.ReadNextSectionAsync();
            }

            return Created(nameof(FileUploadController), null);
        }
    }
}
