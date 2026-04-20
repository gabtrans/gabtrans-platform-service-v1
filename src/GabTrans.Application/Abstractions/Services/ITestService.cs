

namespace GabTrans.Application.Abstractions.Services
{
    public interface ITestService
    {
        Task UploadCountries();
        Task UploadInflowAsync();
    }
}
