using GabTrans.Application.DataTransfer;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace GabTrans.Application.DataTransfer
{
    public class GetAccountRequest: BaseRequest
    {
        public long? RoleId { get; set; }
        public long? CompanyId { get; set; }
        [Display(Name = "Status")]
        public long? ApprovalStageId { get; set; }
        [Display(Name = "Phone Number or Email Address")]
        public string Username { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public bool Complete { get; set; }
    }
}
