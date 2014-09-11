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

namespace Toyota.Common.Web.Platform
{
    public class UserInterfaceSettings
    {
        public bool EnableLayout { set; get; }
        public UILayout GetLayout()
        {
            return ProviderRegistry.Instance.Get<UILayout>();
        }
    }
}
