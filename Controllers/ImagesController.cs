using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BeardMan.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BeardMan.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase


    {
        private readonly IBlobService blobService;

        public ImagesController(IBlobService blobService)
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


    }
}
