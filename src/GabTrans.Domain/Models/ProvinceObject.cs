using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Domain.Models
{
    public class ProvinceObject
    {
        public long Id { get; set; }

        public string Name { get; set; } = null!;

        public string CountryCode { get; set; } = null!;
    }
}
