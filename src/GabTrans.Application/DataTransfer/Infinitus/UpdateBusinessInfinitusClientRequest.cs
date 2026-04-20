using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Application.DataTransfer.Infinitus
{
    public class UpdateBusinessInfinitusClientRequest
    {
        public BusinessInfintusOnboardingDetails onboardingDetails { get; set; }
    }

    public class InfintusBusinessAddress
    {
        public string city { get; set; }
        public string country { get; set; }
        public string postalCode { get; set; }
        public string state { get; set; }
        public string line1 { get; set; }
        public string line2 { get; set; }
    }

    public class InfintusBusinessInfo
    {
        public string name { get; set; }
        public string type { get; set; }
        public string taxId { get; set; }
        public string description { get; set; }
        public string identifier { get; set; }
        public string incorporationDate { get; set; }
        public string tradeName { get; set; }
        public string website { get; set; }
        public string monthlyRevenue { get; set; }
        public string monthlyConversionVolumeDigitalAssets { get; set; }
        public string monthlyLocalPaymentsVolume { get; set; }
        public string monthlyConversionVolumeFiat { get; set; }
        public string monthlySwiftVolume { get; set; }
        public List<string> currencyNeeded { get; set; }
    }

    public class InfintusCountriesOfOperation
    {
        public List<string> northAmerica { get; set; } = new List<string>();
        public List<string> southAmerica { get; set; } = new List<string>();
        public List<string> caribbean { get; set; } = new List<string>();
        public List<string> apac { get; set; } = new List<string>();
        public List<string> middleEast { get; set; } = new List<string>();
        public List<string> europe { get; set; } = new List<string>();
        public List<string> restOfEurope { get; set; } = new List<string>();
        public List<string> africa { get; set; } = new List<string>();
        public List<string> eeaEfta { get; set; } = new List<string>();
    }

    public class InfintusGeneralInfo
    {
        public string mainIndustry { get; set; }
        public string additionalIndustry { get; set; }
        public string contactFirstName { get; set; }
        public string contactLastName { get; set; }
        public string contactEmail { get; set; }
        public string contactPhone { get; set; }
        public string naics { get; set; }
        public string naicsDescription { get; set; }
    }

    public class InfintusMailingAddress
    {
        public string city { get; set; }
        public string country { get; set; }
        public string postalCode { get; set; }
        public string state { get; set; }
        public string line1 { get; set; }
        public string line2 { get; set; }
    }

    public class BusinessInfintusOnboardingDetails
    {
        public string businessCountry { get; set; }
        public InfintusBusinessAddress businessAddress { get; set; }
        public InfintusMailingAddress mailingAddress { get; set; }
        public InfintusBusinessInfo businessInfo { get; set; }
        public InfintusGeneralInfo generalInfo { get; set; }
        public InfintusRegionsOfOperation regionsOfOperation { get; set; }
        public InfintusCountriesOfOperation countriesOfOperation { get; set; }
    }

    public class InfintusRegionsOfOperation
    {
        public bool northAmerica { get; set; }
        public bool europe { get; set; }
        public bool middleEast { get; set; }
        public bool caribbean { get; set; }
        public bool africa { get; set; }
        public bool southAmerica { get; set; }
        public bool apac { get; set; }
        public bool eeaEfta { get; set; }
        public bool restOfEurope { get; set; }
    }
}
