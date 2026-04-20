using GabTrans.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Application.Abstractions.Repositories
{
    public interface IReceiptRepository
    {
        Task<Receipt> DetailsAsync(long transactionId);
    }
}
