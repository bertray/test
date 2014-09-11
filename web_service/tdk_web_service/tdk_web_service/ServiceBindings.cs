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
using System.ServiceModel;

namespace Toyota.Common.Web.Service
{
    public class ServiceBindings
    {
        private ServiceBindings() { }

        public static BasicHttpBinding CreateBasicBinding()
        {
            BasicHttpBinding binding = new BasicHttpBinding();
            return binding;
        }

        public static WSHttpBinding CreateHttpBinding()
        {
            WSHttpBinding binding = new WSHttpBinding();
            return binding;
        }

        public static WebHttpBinding CreateWebBinding()
        {
            WebHttpBinding binding = new WebHttpBinding();
            return binding;
        }

        public static NetTcpBinding CreateNetTcpBinding()
        {
            NetTcpBinding binding = new NetTcpBinding();
            return binding;
        }

        public static NetNamedPipeBinding CreateNetNamedPipeBinding()
        {
            NetNamedPipeBinding binding = new NetNamedPipeBinding();
            return binding;
        }
    }
}
