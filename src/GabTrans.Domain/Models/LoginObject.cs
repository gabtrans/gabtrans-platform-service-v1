using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Domain.Models
{
    public class LoginObject
    {
        public long Id { get; set; }

        public long UserId { get; set; }

        public string SessionToken { get; set; } = null!;

        public bool IsUserLogin { get; set; }

        public DateTime LoginDate { get; set; }

        public long Attempts { get; set; }
    }
}
