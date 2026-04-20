using System;
using GabTrans.Domain.Entities;

namespace GabTrans.Application.DataTransfer
{
    public class GetCrassulaCurrenciesResponse
    {
        public List<CrassulaCurrency> currencies { get; set; }
        public bool countryToCurrencyMappingEnabled { get; set; }
    }

    public class CrassulaCurrency
    {
        public string code { get; set; }
        public List<string> countries { get; set; }
    }
}

