using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using BeardMan.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeardMan.Services
{
    public class BlobService : IBlobService
    {
        private readonly BlobServiceClient blobServiceClient;

        public BlobService(BlobServiceClient blobServiceClient)
        {
            this.blobServiceClient = blobServiceClient;
        }
        
        public async Task<Response<BlobDownloadInfo>> GetBlobAsync(string name)
        {
            var containerClient = blobServiceClient.GetBlobContainerClient("images");
            var blobClient = containerClient.GetBlobClient(name);
            Response<BlobDownloadInfo> blobDownloadInfo = await blobClient.DownloadAsync();

            return blobDownloadInfo;
        }

        public async Task<IEnumerable<string>> ListBlobAsync()
        {
            var containerClient = blobServiceClient.GetBlobContainerClient("images");
            var items = new List<string>();

            await foreach (var blobItem in containerClient.GetBlobsAsync())
            {
                items.Add(blobItem.Name);
            }
            return items;
        }

        public async Task UploadContentBlobAsync(string content, string fileName)
        {
            throw new NotImplementedException();
        }

        public async Task<Response<BlobContentInfo>> UploadFileBlobAsync(string filePath, string fileName)
        {
            var containerClient = blobServiceClient.GetBlobContainerClient("images");
            var blobClient = containerClient.GetBlobClient(fileName);
            Response<BlobContentInfo> blobContentInfo = await blobClient.UploadAsync(filePath, new BlobHttpHeaders { ContentType = filePath.GetContentType() });
            return blobContentInfo;
        }
        
        public Task DeleteBlobAsync(string blobName)
        {
            throw new NotImplementedException();
        }

        Task IBlobService.UploadFileBlobAsync(string filePath, string fileName)
        {
            throw new NotImplementedException();
        }
    }
}
