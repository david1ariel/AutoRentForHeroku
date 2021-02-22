using BeardMan.Models;
using BeardMan.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BeardMan.Controllers
{
    [Route("api/blobs")]
    [ApiController]
    public class BlobExplorerController : ControllerBase
    {
        private readonly IBlobService blobService;

        public BlobExplorerController(IBlobService blobService)
        {
            this.blobService = blobService;
        }

        [HttpGet]
        [Route("{blobName}")]
        public async Task<IActionResult> GetBlob(string blobName)
        {
            try
            {
                var data = await blobService.GetBlobAsync(blobName);

                // Send back the stream to the client:
                return File(data.Value.Content, data.Value.ContentType);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("list")]
        public async Task<IActionResult> ListBlobs()
        {
            try
            {
                return Ok(await blobService.ListBlobAsync());
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        [Route("uploadfile")]
        public async Task<IActionResult> UploadFile([FromBody] UploadFileRequest request)
        {
            try
            {
                await blobService.UploadFileBlobAsync(request.filePath, request.fileName);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
