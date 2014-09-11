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
using System.ServiceModel.Channels;

namespace Toyota.Common.Web.Service
{
    public class ServiceClient: IDisposable
    {
        public ServiceClient(IWebService client)
        {
            Client = client;
        }

        public IWebService Client { set; get; }
        public virtual ServiceResult Execute(ServiceParameter parameter)
        {
            if (!Client.IsNull() && !parameter.IsNull())
            {
                ServiceRuntimeResult runtimeResult = Client.Execute(parameter.ToRuntime());
                if (!runtimeResult.IsNull())
                {
                    return ServiceResult.Create(runtimeResult);
                }
            }
            return null;
        }

        public void Dispose()
        {
            if (!Client.IsNull())
            {
                ((IChannel)Client).Close();
            }
        }
    }
}
