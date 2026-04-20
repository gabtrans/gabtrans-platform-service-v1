using System;
using GabTrans.Application.DataTransfer;
using Microsoft.AspNetCore.Http;

namespace GabTrans.Application.Abstractions.Services;

public interface IAzureStorageService
{
    Task<ApiResponse> UploadAsync(IFormFile file);
    Task<ApiResponse> UploadAsync(Stream stream, string fileName, string contentType);
    Task<ApiResponse> ListAsync();
    Task<ApiResponse> DownloadAsync(string blobFilename);
    Task<ApiResponse> DeleteAsync(string blobFilename);
    Task<string> GenerateSasTokenAsync(string blobName, int expirationMinutes = 60);
}
