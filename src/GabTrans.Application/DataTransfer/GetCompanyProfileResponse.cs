using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Application.DataTransfer
{
    public class GetCompanyProfileResponse
    {
        public List<string> sic_codes { get; set; }
        public RegisteredOfficeAddress registered_office_address { get; set; }
        public string company_name { get; set; }
        public string jurisdiction { get; set; }
        public CompanyAccounts accounts { get; set; }
        public string company_number { get; set; }
        public string last_full_members_list_date { get; set; }
        public string type { get; set; }
        public bool undeliverable_registered_office_address { get; set; }
        public bool has_been_liquidated { get; set; }
        public string date_of_creation { get; set; }
        public string etag { get; set; }
        public string company_status { get; set; }
        public bool has_insolvency_history { get; set; }
        public bool has_charges { get; set; }
        public List<PreviousCompanyName> previous_company_names { get; set; }
        public ConfirmationStatement confirmation_statement { get; set; }
        public CompanyProfileLinks links { get; set; }
        public bool registered_office_is_in_dispute { get; set; }
        public bool has_super_secure_pscs { get; set; }
        public bool can_file { get; set; }
    }


    public class AccountingReferenceDate
    {
        public string day { get; set; }
        public string month { get; set; }
    }

    public class CompanyAccounts
    {
        public AccountingReferenceDate accounting_reference_date { get; set; }
        public string next_due { get; set; }
        public string next_made_up_to { get; set; }
        public NextAccounts next_accounts { get; set; }
        public bool overdue { get; set; }
        public LastAccounts last_accounts { get; set; }
    }

    public class ConfirmationStatement
    {
        public string next_due { get; set; }
        public string next_made_up_to { get; set; }
        public string last_made_up_to { get; set; }
        public bool overdue { get; set; }
    }

    public class LastAccounts
    {
        public string period_start_on { get; set; }
        public string type { get; set; }
        public string made_up_to { get; set; }
        public string period_end_on { get; set; }
    }

    public class CompanyProfileLinks
    {
        public string self { get; set; }
        public string filing_history { get; set; }
        public string officers { get; set; }
        public string exemptions { get; set; }
        public string charges { get; set; }
    }

    public class NextAccounts
    {
        public string period_end_on { get; set; }
        public bool overdue { get; set; }
        public string due_on { get; set; }
        public string period_start_on { get; set; }
    }

    public class PreviousCompanyName
    {
        public string ceased_on { get; set; }
        public string name { get; set; }
        public string effective_from { get; set; }
    }

    public class RegisteredOfficeAddress
    {
        public string address_line_1 { get; set; }
        public string locality { get; set; }
        public string postal_code { get; set; }
        public string address_line_2 { get; set; }
    }
}
