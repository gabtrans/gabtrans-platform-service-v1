using GabTrans.Application.DataTransfer;
using GabTrans.Domain.Entities;
using GabTrans.Domain.Models;


namespace GabTrans.Application.Abstractions.Repositories
{
    public interface IKycRepository
    {
        Task<IEnumerable<Kyc>> GetKycsAsync();
        Task<IEnumerable<Kyc>> GetAsync(string status, string country);
        Task<Kyc> DetailsByUuidAsync(string uuid);
        //Task<List<IdNameObject>> GetProductsAsync(string countryCode);
        Task<CompleteKycObject> GetKycDetailsAsync(long userId);
        Task<Kyc> DetailsAsync(long id);
        Task<bool> UpdateAsync(Kyc kyc);
        Task<Kyc> DetailsByUserAsync(long userId);
        Task<Kyc> DetailsByTaxNumberAsync(string taxNumber);
        Task<IEnumerable<Kyc>> GetCompletedAsync(string status, string country);
    }
}
