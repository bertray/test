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

namespace Toyota.Common.Credential
{
    public class UserCompany: NormalizedData
    {
        public UserCompany()
        {
            Type = UserCompanyType.Unknown;
        }

        public string Alias { set; get; }
        UserCompanyType Type { set; get; }
    }
}
