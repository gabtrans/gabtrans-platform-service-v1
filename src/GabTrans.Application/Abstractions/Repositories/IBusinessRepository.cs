using GabTrans.Application.DataTransfer;
using GabTrans.Domain.Entities;
using GabTrans.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Application.Abstractions.Repositories
{
    public interface IBusinessRepository
    {
        Task<Business> DetailsByIdAsync(long id);
        Task<List<BusinessType>> GetTypesAsync();
        Task<List<BusinessRole>> GetRolesAsync();
        Task<BusinessObject> DetailsByAccountIdAsync(long accountId);
        Task<Business> DetailsAsync(string companyNumber);
        Task<Business> GetByUserAsync(long userId);
        Task<bool> InsertAsync(Business business);
        Task<long> CreateAsync(Business business);
        Task<bool> UpdateAsync(Business business);
        Task<bool> DoesNameExistAsync(string name);
        Task<BusinessAddressModel> GetAddressAsync(long userId);
        Task<BusinessDocumentModel> GetDocumentAsync(long userId);
        Task<BusinessInformationModel> GetInformationAsync(long userId);
    }
}
