using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StreamingApp.Domain.Enums;

public enum OtpPurpose
{
    //why i use this OTP
    EmailVerification = 1,
    PhoneVerification = 2,
    PasswordReset = 3,
    TwoFactorAuthentication = 4
}