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
using System.ServiceModel.Channels;

namespace Toyota.Common.Web.Service
{
    public class ServiceClientFactory<T>: IDisposable
    {
        public ServiceClientFactory(string url) : this(url, null) { }
        public ServiceClientFactory(string url, Binding binding)
        {
            Endpoint = new EndpointAddress(url);
            if (binding == null)
            {
                ServiceBinding = ServiceBindings.CreateBasicBinding();                
            }
            else
            {
                ServiceBinding = binding;
            }            
            Factory = new ChannelFactory<T>(ServiceBinding, Endpoint);
        }

        protected ChannelFactory<T> Factory { set; get; }
        protected EndpointAddress Endpoint { set; get; }
        protected Binding ServiceBinding { private set; get; }

        public T Create()
        {
            return Factory.CreateChannel();
        }

        public virtual void Dispose()
        {
            Factory.Close();
        }
    }
}
