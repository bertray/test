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

namespace Toyota.Common.Credential.TMMIN
{
    internal class TmminUserModel: TmminUser
    {
        public string _CompanyId { set; get; }
        public string _ClassId { set; get; }        
        public string _CostCenterCode { set; get; }
        public string _LocationId { set; get; }
        public string _DefaultSystemId { set; get; }
        public string _Email { set; get; }
        public string _PhoneNumber { set; get; }
        public string _ExtensionPhoneNumber { set; get; }
        public string _MobilePhoneNumber { set; get; }
    }
}
