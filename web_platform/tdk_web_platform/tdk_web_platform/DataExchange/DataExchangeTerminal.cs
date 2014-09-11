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
using Toyota.Common.DataExchange;

namespace Toyota.Common.Web.Platform
{
    public class DataExchangeTerminal
    {
        private DataExchangeTerminal() { }

        public static IDataTerminal GetInstance()
        {
            return ProviderRegistry.Instance.Get<IDataTerminal>();
        }
    }
}
