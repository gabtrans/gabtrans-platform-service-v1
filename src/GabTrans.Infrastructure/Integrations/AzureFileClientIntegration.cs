using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using GabTrans.Application.Abstractions.Integrations;
using GabTrans.Application.Abstractions.Logging;
using GabTrans.Application.Abstractions.Services;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Infrastructure.Integrations
{
    public class AzureFileClientIntegration(ILogService logService,BlobServiceClient blobServiceClient, IWebHostEnvironment hostingEnvironment) : IAzureFileClientIntegration
    {
        private readonly ILogService _logService = logService;
        private readonly BlobServiceClient _blobServiceClient = blobServiceClient;
        private readonly IWebHostEnvironment _hostingEnvironment = hostingEnvironment;
        public async Task<string> UploadFileAsync(FileStream fileStream,string fileExtension)
        {
            try
            {
                var containerClient = _blobServiceClient.GetBlobContainerClient("gabtrans-files");
                await containerClient.CreateIfNotExistsAsync(PublicAccessType.Blob); // Create if not present

                var blobClient = containerClient.GetBlobClient(Guid.NewGuid().ToString() + fileExtension); // Generate a unique name

                await blobClient.UploadAsync(fileStream, true);
                return blobClient.Uri.ToString();
            }
            catch (Exception ex)
            {
                _logService.LogError("FileService", "UploadFileAsync", ex);
            }

            return string.Empty;
        }

        public async Task<Stream> DownloadFileAsync(string fileName)
        {
            try
            {
                //var file = ReadFileToFileStream(filePath);
                //if (file == null) return string.Empty;

                fileName = "https://gabtrans.blob.core.windows.net/gabtrans-files/d815f83a-f6ef-4882-a9d4-8a7400cd710d.png";
                string[] files = fileName.Split('/');

                fileName = files[4];

                var containerClient = _blobServiceClient.GetBlobContainerClient("gabtrans-files");

                var blobClient = containerClient.GetBlobClient(fileName);
                if (!await blobClient.ExistsAsync())
                {
                    return null; // File not found
                }

                // string path = $"C:Users\\MY SPECTRE\\Desktop\\Test\\Test\\{fileName}";
                // await blobClient.DownloadToAsync(path);

                var stream = new MemoryStream();
                await blobClient.DownloadToAsync(stream);
                stream.Position = 0;
                return stream;
            }
            catch (Exception ex)
            {
                _logService.LogError("FileService", "UploadFileAsync", ex);
            }

            return null;
        }
    }
}
