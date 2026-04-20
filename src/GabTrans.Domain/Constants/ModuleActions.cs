using GabTrans.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Domain.Constants
{
    public static class ModuleActions
    {
        public const long Update_Address = 1;
        public const long Update_Document = 2;
        public const long Update_Information = 3;
        public const long Add_New_Fee = 4;
        public const long Update_Fee = 5;
        public const long Get_Fees = 6;
        public const long Add_New_FX_rate = 7;
        public const long Generate_FX_Rate = 8;
        public const long Trade_FX_conversion = 9;
        public const long Trade_Crypto = 10;
        public const long Update_Address_Information = 11;
        public const long Update_Employment_Information = 12;
        public const long Update_Identity_Information = 13;
        public const long Update_Income_Information = 14;
        public const long Update_Personal_Information = 15;
        public const long Update_Account_Request = 16;
        public const long Suspend_Account = 17;
        public const long View_Account_Requests = 18;
        public const long Add_Limit = 19;
        public const long Update_Limit = 20;
        public const long Get_Limits = 21;
        public const long Generate_Otp = 22;
        public const long Initate_Transfer = 23;
        public const long Update_Transfer = 24;
        public const long Get_Recipients = 25;
        public const long Delete_Recipient = 26;
        public const long Update_Representative_Address_Information = 27;
        public const long Update_Representative_Employment_Information = 28;
        public const long Update_Representative_Identity_Information = 29;
        public const long Update_Representative_Income_Information = 30;
        public const long Update_Representative_Personal_Information = 31;
        public const long Sign_In = 32;
        public const long Change_Password = 33;
        public const long Forget_Password = 34;
        public const long Reset_Password = 35;
        public const long Refresh_Token = 36;
        public const long Generate_Token = 37;
        public const long Initiate_Signup = 38;
        public const long Send_Invitation = 39;
        public const long Accept_Invitation = 40;
        public const long Verify_Signup_Email = 41;
        public const long Complete_Signup = 42;
        public const long Create_Transaction_Pin = 43;
        public const long Update_Transaction_Pin = 44;
        public const long View_Account_Details = 45;
        public const long Update_User_Details = 46;
        public const long Get_Users = 47;
        public const long Get_User_Details = 48;
        public const long Validate_Otp = 49;
        public const long Update_User_Role = 50;
    }
}
