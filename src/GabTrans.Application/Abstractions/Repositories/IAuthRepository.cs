using GabTrans.Application.DataTransfer;
using GabTrans.Domain.Entities;


namespace GabTrans.Application.Abstractions.Repositories
{
    public interface IAuthRepository
    {
        Task<bool> InsertAsync(AuthCredential authCredential);
        Task<bool> ValidateAsync(string token);
        Task<AuthCredential> DetailsAsync(long id);
        Task<bool> ValidateAsync(string username, string password);
    }
}
