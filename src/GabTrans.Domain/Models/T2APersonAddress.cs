using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Domain.Models
{
    public class T2APersonAddress
    {
        public List<T2APersonList> person_list { get; set; }
        public string mode { get; set; }
        public string chargeable { get; set; }
        public string total_records { get; set; }
        public string t2a_version_number { get; set; }
        public string status { get; set; }
        public string error_code { get; set; }
        public string credit_used { get; set; }
    }

    public class T2ADobDetails
    {
        public string y { get; set; }
        public string m { get; set; }
        public string en { get; set; }
    }

    public class T2APersonList
    {
        public string years_text { get; set; }
        public List<string> years_list { get; set; }
        public string title { get; set; }
        public string forename { get; set; }
        public string middle_initial { get; set; }
        public string surname { get; set; }
        public string name_single_line { get; set; }
        public string director { get; set; }
        public string dob { get; set; }
        public T2ADobDetails dob_details { get; set; }
        public string address_id { get; set; }
        public string line_1 { get; set; }
        public string line_2 { get; set; }
        public string line_3 { get; set; }
        public string place { get; set; }
        public string town { get; set; }
        public string postcode { get; set; }
        public string addr_single_line { get; set; }
        public string address_key { get; set; }
    }
}
