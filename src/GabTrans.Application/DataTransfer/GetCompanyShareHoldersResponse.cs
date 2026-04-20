using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Application.DataTransfer
{
    public class GetCompanyShareHoldersResponse
    {
        public int ceased_count { get; set; }
        public List<CompanyShareHoldersItem> items { get; set; }
        public int items_per_page { get; set; }
        public CompanyShareHoldersLinks links { get; set; }
        public int active_count { get; set; }
        public int start_index { get; set; }
        public int total_results { get; set; }
    }

    public class CompanyShareHoldersAddress
    {
        public string premises { get; set; }
        public string address_line_1 { get; set; }
        public string locality { get; set; }
        public string country { get; set; }
        public string region { get; set; }
    }

    public class CompanyShareHoldersDateOfBirth
    {
        public int year { get; set; }
        public int month { get; set; }
    }

    public class CompanyShareHoldersItem
    {
        public string name { get; set; }
        public string kind { get; set; }
        public string nationality { get; set; }
        public CompanyShareHoldersLinks links { get; set; }
        public CompanyShareHoldersAddress address { get; set; }
        public string etag { get; set; }
        public CompanyShareHoldersNameElements name_elements { get; set; }
        public string notified_on { get; set; }
        public List<string> natures_of_control { get; set; }
        public bool is_sanctioned { get; set; }
        public CompanyShareHoldersDateOfBirth date_of_birth { get; set; }
    }

    public class CompanyShareHoldersLinks
    {
        public string self { get; set; }
        public string persons_with_significant_control_statements { get; set; }
    }

    public class CompanyShareHoldersNameElements
    {
        public string forename { get; set; }
        public string surname { get; set; }
    }
}
