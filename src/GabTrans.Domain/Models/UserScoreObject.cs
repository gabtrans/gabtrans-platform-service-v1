using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Domain.Models
{
    public class UserScoreObject
    {
        public long AccountType { get; set; }
        public string AccountTypeValue { get; set; }
        public long Profession { get; set; }
        public long ProfessionId { get; set; }
        public string ProfessionValue { get; set; }
        public long CountryOfOrigin { get; set; }
        public string CountryOfOriginValue { get; set; }
        public long CountryOfResidence { get; set; }
        public string CountryOfResidenceValue { get; set; }
        public long PresenceOfPEP { get; set; }
        public string PresenceOfPEPValue { get; set; }
        public long AdverseMedia { get; set; }
        public string AdverseMediaValue { get; set; }
        public long BusinessSector1 { get; set; }
        public string BusinessSector1Value { get; set; }
        public string BusinessSector1Grade { get; set; }
        public long BusinessSector2 { get; set; }
        public string BusinessSector2Value { get; set; }
        public string BusinessSector2Grade { get; set; }
        public long UBOBusinessSector { get; set; }
        public long UBOCountryOfResidence { get; set; }
        public string UBOCountryOfResidenceCode { get; set; }
        public string UBOBusinessSectorValue { get; set; }
        public string UBOBusinessSectorGrade { get; set; }
        public long IsCompanyOwnerPEP { get; set; }
        public string IsCompanyOwnerPEPValue { get; set; }
        public string IsCompanyOwnerPEPGrade { get; set; }
        public long NomineeShareholder { get; set; }
        public string NomineeShareholderValue { get; set; }
        public string NomineeShareholderGrade { get; set; }
        public long CompanyActivities { get; set; }
        public string CompanyActivitiesValue { get; set; }
        public string CompanyActivitiesGrade { get; set; }
        public long OwnerShipStructure { get; set; }
        public string OwnerShipStructureValue { get; set; }
        public string OwnerShipStructureGrade { get; set; }
        public long ProductsAndServices { get; set; }
        public string ProductsAndServicesValue { get; set; }
        public long ProvenanceOfFund { get; set; }
        public string ProvenanceOfFundValue { get; set; }
        public long DestinationOfFund { get; set; }
        public string DestinationOfFundValue { get; set; }
        public long DateOfCorporation { get; set; }
        public DateTime? DateOfCorporationValue { get; set; }
        public string DateOfCorporationGrade { get; set; }
        public long TransactionVolume { get; set; }
        public string TransactionVolumeValue { get; set; }
        //public long UBOCountryOfResidence { get; set; }
        public string UBOCountryOfResidenceValue { get; set; }
        public string StateOfOriginValue { get; set; }
        public long StateOfOrigin { get; set; }
        public long BvnValidation { get; set; }
    }
}
