///
/// <author>lufty.abdillah@gmail.com</author>
/// <summary>
/// Toyota .Net Development Kit
/// Copyright (c) Toyota Motor Manufacturing Indonesia, All Right Reserved.
/// </summary>
///
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Toyota.Common.Utilities;

namespace Toyota.Common.SSO
{
    internal class DefaultPolicy: ISSOPolicy
    {
        public SSOPolicyState Evaluate(SSOLoginInfo info, DateTime time)
        {
            int timediff = -1;
            if (!info.LastActive.IsNull() && (info.LastActive > DateTime.MinValue))
            {
                timediff = time.Subtract(info.LastActive).Minutes;
            }
            else
            {
                timediff = time.Subtract(info.LoginTime).Minutes;
            }
            if (timediff >= info.SessionTimeout)
            {
                return SSOPolicyState.SessionExpired;
            }

            if (!info.LastActive.IsNull() && (info.LastActive > DateTime.MinValue))
            {
                timediff = time.Subtract(info.LastActive).Minutes;
            }
            else
            {
                timediff = time.Subtract(info.UnlockTime).Minutes;
            }
            if (timediff >= info.LockTimeout)
            {
                return SSOPolicyState.LockActive;
            }

            return SSOPolicyState.Hold;
        }
    }
}
