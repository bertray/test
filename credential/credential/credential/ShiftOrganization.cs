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

namespace Toyota.Common.Credential
{
    public class ShiftOrganization
    {
        public string Code { set; get; }
        public string Name { set; get; }

        public ShiftOrganizationType Type { set; get; }
    }
}
