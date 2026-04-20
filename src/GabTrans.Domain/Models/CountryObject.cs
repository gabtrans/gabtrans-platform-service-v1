using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Domain.Models
{
    public class CountryObject
    {
        public long Id { get; set; }

        public string Name { get; set; } = null!;

        public string Code { get; set; } = null!;

        public string Code2 { get; set; } = null!;

        public string Dial { get; set; } = null!;

        public string? Currency { get; set; }

        public string? CurrencyName { get; set; }

        public string? Flag { get; set; }

        public bool IsActive { get; set; }

        public string Language { get; set; } = null!;

        public string? TimeZone { get; set; }

        public bool AllowTransfer { get; set; }

        public bool Subsidiary { get; set; }

        public long? Score { get; set; }

        public long? RiskLevelId { get; set; }
    }
}
