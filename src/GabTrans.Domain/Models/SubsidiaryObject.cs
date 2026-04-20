using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Domain.Models
{
    public class SubsidiaryObject
    {
        public uint Id { get; set; }
        public string Name { get; set; } = null!;
        public string Iso { get; set; } = null!;
        public string Iso3 { get; set; } = null!;
        public string Dial { get; set; } = null!;
        public string? Currency { get; set; }
        public string? CurrencyName { get; set; }
        public string? Flag { get; set; }
        public string Subsidiary { get; set; }
    }
}
