using GabTrans.Domain.Entities;

namespace GabTrans.Application.Abstractions.Services
{
    public interface IGRemitService
    {
        Task ProcessAsync();
        Task ConfirmationAsync();
    }
}
