using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Application.DataTransfer
{
    public class PaginatedResponse : ApiResponse
    {
        public long PageNumber { get; init; }
        public long PageSize { get; init; }
        public long TotalPages { get; init; }
        public long TotalRecords { get; init; }
        public bool HasNexPage => PageNumber * PageSize < TotalRecords;
        public bool HasPreviousPage => PageSize > 1;

        public PaginatedResponse(object data, long pageNumber, long pageSize, long totalRecords, string message, bool success = false)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            TotalPages = (long)Math.Ceiling((decimal)totalRecords / (decimal)pageSize);
            TotalRecords = totalRecords;
            Success = success;
            Data = data;
            Message = message;
        }
    }
}
