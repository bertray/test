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
    public class BaseServiceClient<T>: IDisposable
    {
        public BaseServiceClient(T client)
        {
            Client = client;
        }

        public T Client { private set; get; }

        public virtual void Dispose()
        {
            if (!Client.IsNull())
            {
                ((IChannel)Client).Close();
            }
        }
    }
}
