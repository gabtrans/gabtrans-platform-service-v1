using GabTrans.Application.DataTransfer;
using GabTrans.Domain.Constants;
using GabTrans.Domain.Entities;
using GabTrans.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Application.Abstractions.Repositories
{
    public interface IRepresentativeRepository
    {
        Task<bool> InsertAsync(Representative representative);
        Task<bool> UpdateAsync(Representative representative);
        Task<RepresentativeModel> DetailsAsync(long Id);
        Task<Representative> DetailsByIdAsync(long Id);
        Task<List<RepresentativeModel>> GetAsync(GetRepresentativeRequest request);
        Task<List<RepresentativeModel>> GetAsync(long businessId);
        Task<List<Representative>> GetByBusinessAsync(long businessId);
    }
}
