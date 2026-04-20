using GabTrans.Application.DataTransfer;
using GabTrans.Domain.Entities;
using GabTrans.Domain.Models;


namespace GabTrans.Application.Abstractions.Repositories
{
    public interface ICurrencyRepository
    {
        Task<CurrencyModel> DetailsAsync(long Id);
        Task<Currency> DetailsByCodeAsync(string currencyCode);
        Task<Currency> DetailsByCountryAsync(string countryCode);
        Task<List<Currency>> GetAsync();
        Task<List<CurrencyModel>> GetAllAsync();
        //Task<List<CurrencyObject>> PendingAsync(long accountId);
    }
}
