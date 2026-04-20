using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Application.DataTransfer
{
    public class GetCompanyResponse
    {
        public string etag { get; set; }
        public List<CompanyItem> items { get; set; }
        public string items_per_page { get; set; }
        public string kind { get; set; }
        public string start_index { get; set; }
        public string total_results { get; set; }
    }

    public class Address
    {
        public string address_line_1 { get; set; }
        public string address_line_2 { get; set; }
        public string care_of { get; set; }
        public string country { get; set; }
        public string locality { get; set; }
        public string po_box { get; set; }
        public string postal_code { get; set; }
        public string region { get; set; }
        public string premises { get; set; }
    }

    public class CompanyItem
    {
        public string title { get; set; }
        public string snippet { get; set; }
        public string address_snippet { get; set; }
        public string kind { get; set; }
        public string company_number { get; set; }
        public string date_of_creation { get; set; }
        public CompanyLinks links { get; set; }
        public string company_status { get; set; }
        public CompanyMatches matches { get; set; }
        public List<string> description_identifier { get; set; }
        public Address address { get; set; }
        public string company_type { get; set; }
        public string description { get; set; }
        public string external_registration_number { get; set; }
    }

    public class CompanyLinks
    {
        public string self { get; set; }
    }

    public class CompanyMatches
    {
        public List<int> title { get; set; }
        public List<int> snippet { get; set; }
    }
}
