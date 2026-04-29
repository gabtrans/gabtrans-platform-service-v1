using GabTrans.Domain.Entities;

namespace GabTrans.Application.Abstractions.Services
{
    public interface IGRemitService
    {
        Task FetchAsync();
        Task ProcessAsync();
        Task NotifyAsync();
    }
}
