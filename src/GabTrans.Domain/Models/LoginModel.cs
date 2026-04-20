using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Domain.Models
{
    public class LoginModel
    {
        public long UserId { get; set; }
        public string RefreshToken { get; set; }
        public DateTime Expires { get; set; }
        public string SessionToken { get; set; }
        public string Status { get; set; }
        public DateTime LastAccessed { get; set; }
        public long Attempts { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        // public string IpAddress { get; set; }
    }
}
