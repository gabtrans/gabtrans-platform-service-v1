using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Application.DataTransfer
{
    public class GetCompanyOfficers
    {
        public int resigned_count { get; set; }
        public OfficerLinks links { get; set; }
        public int items_per_page { get; set; }
        public int total_results { get; set; }
        public List<OfficerItem> items { get; set; }
        public int start_index { get; set; }
        public int inactive_count { get; set; }
        public string kind { get; set; }
        public string etag { get; set; }
        public int active_count { get; set; }
    }


    //public class OfficerAddress
    //{
    //    public string premises { get; set; }
    //    public string address_line_1 { get; set; }
    //    public string country { get; set; }
    //    public string postal_code { get; set; }
    //    public string address_line_2 { get; set; }
    //    public string locality { get; set; }
    //    public string region { get; set; }
    //}

    public class OfficerDateOfBirth
    {
        public int month { get; set; }
        public int year { get; set; }
    }

    public class OfficerItem
    {
        public string officer_role { get; set; }
        public string appointed_on { get; set; }
        public Address address { get; set; }
        public string name { get; set; }
        public OfficerLinks links { get; set; }
        public string country_of_residence { get; set; }
        public OfficerDateOfBirth date_of_birth { get; set; }
        public string occupation { get; set; }
        public string nationality { get; set; }
        public string resigned_on { get; set; }
    }

    public class OfficerLinks
    {
        public string self { get; set; }
        public Officer officer { get; set; }
    }

    //public class Officer
    //{
    //    public string appointments { get; set; }
    //}

}
