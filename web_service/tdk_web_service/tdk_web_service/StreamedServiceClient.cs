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
    public class StreamedServiceClient: IDisposable
    {
        public StreamedServiceClient(IStreamedWebService client)
        {
            Client = client;
        }

        public IStreamedWebService Client { set; get; }

        public virtual StreamedServiceResult Execute(StreamedServiceParameter parameter)
        {
            if (!Client.IsNull() && !parameter.IsNull())
            {
                StreamedServiceRuntimeResult runtimeResult = Client.Execute(parameter.ToRuntime());
                if (!runtimeResult.IsNull())
                {
                    return StreamedServiceResult.Create(runtimeResult);
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
