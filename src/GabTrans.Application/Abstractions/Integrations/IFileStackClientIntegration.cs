using GabTrans.Application.DataTransfer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Application.Abstractions.Integrations
{
    public interface IFileStackClientIntegration
    {
        Task<FileStackFileUploadResponse> Upload(string imageUrl);
        Task<FileStackFileUploadResponse> Upload(byte[] imageFile);
        Task<FileStackFileUploadResponse> Upload(byte[] imageFile, string contentType);
        Task<string> GetContentType(string imagePath);
    }
}
