using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Domain.Enums
{
    public enum OTPCategories
    {
        EmailVerification = 1,
        Authorization = 2,
        ChangePassword = 3,
        Login = 4,
        CompleteSignUp = 5
    }
}
