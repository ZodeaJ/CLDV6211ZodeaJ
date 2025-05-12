using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs;

namespace WebAppPart1.Helpers
{
    public class AzureBlobHelper
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly string _connectionString;

        public AzureBlobHelper(string connectionString)
        {
            _connectionString = connectionString;
        }
        public AzureBlobHelper(BlobServiceClient blobServiceClient)
        {
            _blobServiceClient = blobServiceClient;
        }

        public async Task<List<string>> GetBlobUrlsAsync(string containerName)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            var urls = new List<string>();

            await foreach (var blobItem in containerClient.GetBlobsAsync())
            {
                var uri = containerClient.GetBlobClient(blobItem.Name).Uri;
                urls.Add(uri.ToString());
            }

            return urls;
        }

        public async Task DeleteFileAsync(string fileName, string containerName)
        {
            try
            {
                BlobServiceClient blobServiceClient = new BlobServiceClient(_connectionString);
                BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);

                BlobClient blobClient = containerClient.GetBlobClient(fileName);
                await blobClient.DeleteIfExistsAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Blob delete failed: " + ex.Message);
            }
        }
    }
}