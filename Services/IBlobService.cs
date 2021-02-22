using Azure;
using Azure.Storage.Blobs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeardMan.Services
{
    public interface IBlobService
    {
        public Task<Response<BlobDownloadInfo>> GetBlobAsync(string name);
        
        public Task<IEnumerable<string>> ListBlobAsync();

        public Task UploadFileBlobAsync(string filePath, string fileName);
        
        public Task UploadContentBlobAsync(string content, string fileName);
        
        public Task DeleteBlobAsync(string blobName);


    }
}
