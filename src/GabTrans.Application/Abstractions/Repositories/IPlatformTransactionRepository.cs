

using GabTrans.Domain.Entities;

namespace GabTrans.Application.Abstractions.Repositories;

public interface IPlatformTransactionRepository
{
    //Task<bool> UpdateApplicationAsync(long id);
    Task<IEnumerable<PlatformTransaction>> GetAsync();
    Task<IEnumerable<PlatformTransaction>> GetPendingAsync();
    Task<bool> UpdateStatusAsync(long id, string status);
    Task<IEnumerable<PlatformTransaction>> GetByStatusAsync(string status);
    Task<IEnumerable<PlatformTransaction>> GetAsync(string status, List<long> accountIds);
    Task<IEnumerable<PlatformTransaction>> GetAsync(long accountId);
    Task<IEnumerable<PlatformTransaction>> GetAsync(long accountId, string status);
    Task<bool> CloseAsync(string reference, string status);
    Task<PlatformTransaction> DetailsAsync(string reference);
    Task<PlatformTransaction> DetailsAsync(long id);
    Task<bool> UpdateAsync(PlatformTransaction platformTransaction);
    Task<bool> UpdateStatusAsync(string reference, string status, string? response = null);
    Task<bool> InsertAsync(long accountId, string reference, string request, string gateway);
    Task<bool> BulkInsertAsync(List<PlatformTransaction> gatewayPayouts);
    Task<IEnumerable<PlatformTransaction>> GetPendingAsync(long accountId);
    Task<IEnumerable<GremitAccount>> GetGRemitAsync(string status);
}
