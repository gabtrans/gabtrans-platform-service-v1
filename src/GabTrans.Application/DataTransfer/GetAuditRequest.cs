using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Application.DataTransfer
{
    public class GetAuditRequest
    {
        public long? UserId { get; set; }
        public long? ModuleId { get; set; }
        public string? Email { get; set; }
        public string? StartDate { get; set; }
        public string? EndDate { get; set; }
        public int PageSize { get; set; } = 20;
        public int PageNumber { get; set; } = 1;
    }
}
