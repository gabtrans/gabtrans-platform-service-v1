using System;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Sas;
using GabTrans.Application.Abstractions.Logging;
using GabTrans.Application.Abstractions.Services;
using GabTrans.Application.DataTransfer;
using GabTrans.Domain.Models;
using Microsoft.AspNetCore.Http;

namespace GabTrans.Application.Services;

public class AzureStorageService : IAzureStorageService
{
    private readonly ILogService _logService;
    private readonly BlobServiceClient _blobServiceClient;

    public AzureStorageService(ILogService logService)
    {
        _blobServiceClient = new BlobServiceClient(StaticData.StorageConnectionStrings);
        _logService = logService;

        // Create container if it doesn't exist
        CreateContainerIfNotExists().Wait();
    }

    private async Task CreateContainerIfNotExists()
    {
        try
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(StaticData.StorageContainer);
            await containerClient.CreateIfNotExistsAsync(PublicAccessType.Blob);
        }
        catch (Exception ex)
        {
            _logService.LogError(nameof(AzureStorageService), nameof(CreateContainerIfNotExists), ex);
        }
    }

    public async Task<ApiResponse> UploadAsync(IFormFile file)
    {
        ApiResponse response = new();

        try
        {
            // Validate file
            if (file == null || file.Length == 0)
            {
                response.Success = false;
                response.Message = "File is empty";
                return response;
            }

            // Generate unique filename
            var fileName = $"{Guid.NewGuid()}_{Path.GetFileName(file.FileName)}";

            // Get container client
            var containerClient = _blobServiceClient.GetBlobContainerClient(StaticData.StorageContainer);
            var blobClient = containerClient.GetBlobClient(fileName);

            // Set content type
            var blobHttpHeaders = new BlobHttpHeaders
            {
                ContentType = file.ContentType
            };

            // Upload file
            await using (var stream = file.OpenReadStream())
            {
                await blobClient.UploadAsync(stream, new BlobUploadOptions
                {
                    HttpHeaders = blobHttpHeaders
                });
            }

            // Return success response
            response.Success = true;
            response.Message = "File uploaded successfully";
            response.Data = new AzureUploadedFile { Uri = blobClient.Uri.AbsoluteUri, Name = fileName, ContentType = file.ContentType };

            _logService.LogInfo(nameof(AzureStorageService), nameof(UploadAsync), $"File uploaded: {fileName}");
        }
        catch (Exception ex)
        {
            _logService.LogError(nameof(AzureStorageService), nameof(UploadAsync), ex);
            response.Success = false;
            response.Message = $"Error: {ex.Message}";
        }

        return response;
    }

    public async Task<ApiResponse> UploadAsync(Stream stream, string fileName, string contentType)
    {
        ApiResponse response = new();

        try
        {
            // Generate unique filename
            var uniqueFileName = $"{Guid.NewGuid()}_{fileName}";

            // Get container client
            var containerClient = _blobServiceClient.GetBlobContainerClient(StaticData.StorageContainer);
            var blobClient = containerClient.GetBlobClient(uniqueFileName);

            // Set content type
            var blobHttpHeaders = new BlobHttpHeaders
            {
                ContentType = contentType
            };

            // Upload stream
            await blobClient.UploadAsync(stream, new BlobUploadOptions
            {
                HttpHeaders = blobHttpHeaders
            });

            response.Success = true;
            response.Message = "File uploaded successfully";
            response.Data = new AzureUploadedFile { Uri = blobClient.Uri.AbsoluteUri, Name = uniqueFileName, ContentType = contentType };

            _logService.LogInfo(nameof(AzureStorageService), nameof(UploadAsync), $"Stream uploaded: {uniqueFileName}");
        }
        catch (Exception ex)
        {
            _logService.LogError(nameof(AzureStorageService), nameof(UploadAsync), ex);
            response.Success = false;
            response.Message = $"Error: {ex.Message}";
        }

        return response;
    }

    public async Task<ApiResponse> ListAsync()
    {
        ApiResponse response = new();
        var files = new List<AzureUploadedFile>();

        try
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(StaticData.StorageContainer);

            await foreach (var blobItem in containerClient.GetBlobsAsync())
            {
                var blobClient = containerClient.GetBlobClient(blobItem.Name);

                files.Add(new AzureUploadedFile
                {
                    Uri = blobClient.Uri.AbsoluteUri,
                    Name = blobItem.Name,
                    ContentType = blobItem.Properties.ContentType,
                    Size = blobItem.Properties.ContentLength,
                    LastModified = blobItem.Properties.LastModified
                });
            }

            response.Data = files;
        }
        catch (Exception ex)
        {
            _logService.LogError(nameof(AzureStorageService), nameof(ListAsync), ex);
            response.Success = false;
            response.Message = $"Error: {ex.Message}";
        }

        return response;
    }

    public async Task<ApiResponse> DownloadAsync(string blobFilename)
    {
        ApiResponse response = new();

        try
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(StaticData.StorageContainer);
            var blobClient = containerClient.GetBlobClient(blobFilename);

            if (!await blobClient.ExistsAsync())
            {
                response.Success = false;
                response.Message = "File not found";
                return response;
            }

            var download = await blobClient.DownloadAsync();

            response.Success = true;
            response.Message = "File downloaded successfully";
            response.Data = download.Value.ContentType;
            //response.Data = new AzureUploadedFile { Uri = blobClient.Uri.AbsoluteUri, Name = blobFilename, ContentType = download.Value.ContentType };

            _logService.LogInfo(nameof(AzureStorageService), nameof(DownloadAsync), $"File downloaded: {blobFilename}");
        }
        catch (Exception ex)
        {
            _logService.LogError(nameof(AzureStorageService), nameof(DownloadAsync), ex);
            response.Success = false;
            response.Message = $"Error: {ex.Message}";
        }

        return response;
    }

    public async Task<ApiResponse> DeleteAsync(string blobFilename)
    {
        ApiResponse response = new();

        try
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(StaticData.StorageContainer);
            var blobClient = containerClient.GetBlobClient(blobFilename);

            if (!await blobClient.ExistsAsync())
            {
                response.Success = false;
                response.Message = "File not found";
                return response;
            }

            await blobClient.DeleteAsync();

            response.Success = true;
            response.Message = "File deleted successfully";
            response.Data = blobFilename;

            _logService.LogInfo(nameof(AzureStorageService), nameof(DeleteAsync), $"File deleted: {blobFilename}");
        }
        catch (Exception ex)
        {
            _logService.LogError(nameof(AzureStorageService), nameof(DeleteAsync), ex);
            response.Success = false;
            response.Message = $"Error: {ex.Message}";
        }

        return response;
    }

    public async Task<string> GenerateSasTokenAsync(string blobName, int expirationMinutes = 60)
    {
        try
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(StaticData.StorageContainer);
            var blobClient = containerClient.GetBlobClient(blobName);

            if (!await blobClient.ExistsAsync())
            {
                return null;
            }

            var sasBuilder = new BlobSasBuilder
            {
                BlobContainerName = StaticData.StorageContainer,
                BlobName = blobName,
                Resource = "b",
                ExpiresOn = DateTimeOffset.UtcNow.AddMinutes(expirationMinutes)
            };

            sasBuilder.SetPermissions(BlobSasPermissions.Read);

            var sasToken = blobClient.GenerateSasUri(sasBuilder);
            return sasToken.AbsoluteUri;
        }
        catch (Exception ex)
        {
            _logService.LogError(nameof(AzureStorageService), nameof(GenerateSasTokenAsync), ex);
            return null;
        }
    }
}
