using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Application.Abstractions.Integrations
{
    public interface IAzureFileClientIntegration
    {
        Task<Stream> DownloadFileAsync(string fileName);
        Task<string> UploadFileAsync(FileStream fileStream, string fileExtension);
    }
}
