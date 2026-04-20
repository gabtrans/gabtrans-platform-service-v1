using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Application.DataTransfer
{
    public class NotifyDirectorRequest : BaseRequest
    {
        public long DirectorId { get; set; }
        public string EmailAddress { get; set; }
        public string ResidentialAddress { get; set; }
    }
}
