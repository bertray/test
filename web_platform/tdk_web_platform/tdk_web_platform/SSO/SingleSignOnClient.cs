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
using Toyota.Common.Web.Service;
using Toyota.Common.Utilities;

namespace Toyota.Common.Web.Platform
{
    public class SingleSignOnClient: IDisposable
    {
        private SingleSignOnClient() 
        {
            ServiceFactory = new ServiceClientFactory<IWebService>(ApplicationSettings.Instance.Security.SingleSignOnServiceUrl);
        }

        private static SingleSignOnClient instance = null;
        public static SingleSignOnClient Instance
        {
            get
            {
                if (instance.IsNull())
                {
                    instance = new SingleSignOnClient();
                }
                return instance;
            }
        }

        private ServiceClientFactory<IWebService> ServiceFactory { set; get; }

        public bool IsUserAuthentic(string username, string password)
        {
            ServiceParameter param = new ServiceParameter().Define(p =>
            {
                p.Command = "IsUserAuthentic";
                p.Parameters.Add("username", username);
                p.Parameters.Add("password", password);
            });
            IWebService service = ServiceFactory.Create();
            ServiceRuntimeResult runtimeResult = service.Execute(param.ToRuntime());
            if (!runtimeResult.IsNull())
            {
                ServiceResult result = ServiceResult.Create(runtimeResult);
                if (result.Status == ServiceStatus.Success)
                {
                    return result.Data.Get<bool>("IsUserAuthentic");
                }
            }
            service.Dispose();
            return false;
        }

        public bool Login(string username, string password)
        {
            ServiceParameter param = new ServiceParameter().Define(p => {
                p.Command = "Login";
                p.Parameters.Add("username", username);
                p.Parameters.Add("password", password);
            });
            IWebService service = ServiceFactory.Create();
            ServiceRuntimeResult runtimeResult = service.Execute(param.ToRuntime());
            if (!runtimeResult.IsNull())
            {
                ServiceResult result = ServiceResult.Create(runtimeResult);
                return (result.Status == ServiceStatus.Success);
            }
            service.Dispose();
            return false;
        }

        public string IsUserLoggedIn(string username, string password)
        {
            string id = null;
            ServiceParameter param = new ServiceParameter().Define(p =>
            {
                p.Command = "IsUserLoggedIn";
                p.Parameters.Add("username", username);
                p.Parameters.Add("password", password);
            });
            IWebService service = ServiceFactory.Create();
            ServiceRuntimeResult runtimeResult = service.Execute(param.ToRuntime());
            if (!runtimeResult.IsNull())
            {
                ServiceResult result = ServiceResult.Create(runtimeResult);
                if (result.Status == ServiceStatus.Success)
                {
                    id = result.Data.Get<string>("Id");
                }
            }
            service.Dispose();
            return id;
        }

        public bool IsUserLocked(string username, string password)
        {
            ServiceParameter param = new ServiceParameter().Define(p =>
            {
                p.Command = "IsUserLocked";
                p.Parameters.Add("username", username);
                p.Parameters.Add("password", password);
            });
            IWebService service = ServiceFactory.Create();
            ServiceRuntimeResult runtimeResult = service.Execute(param.ToRuntime());
            if (!runtimeResult.IsNull())
            {
                ServiceResult result = ServiceResult.Create(runtimeResult);
                if (result.Status == ServiceStatus.Success)
                {
                    return result.Data.Get<bool>("IsUserLocked");
                }
            }
            service.Dispose();
            return false;
        }

        public bool Lock(string username, string password)
        {
            ServiceParameter param = new ServiceParameter().Define(p =>
            {
                p.Command = "IsUserLocked";
                p.Parameters.Add("username", username);
                p.Parameters.Add("password", password);
            });
            IWebService service = ServiceFactory.Create();
            ServiceRuntimeResult runtimeResult = service.Execute(param.ToRuntime());
            if (!runtimeResult.IsNull())
            {
                ServiceResult result = ServiceResult.Create(runtimeResult);
                return (result.Status == ServiceStatus.Success);
            }
            service.Dispose();
            return false;
        }

        public bool Unlock(string username, string password)
        {
            ServiceParameter param = new ServiceParameter().Define(p =>
            {
                p.Command = "Unlock";
                p.Parameters.Add("username", username);
                p.Parameters.Add("password", password);
            });
            IWebService service = ServiceFactory.Create();
            ServiceRuntimeResult runtimeResult = service.Execute(param.ToRuntime());
            if (!runtimeResult.IsNull())
            {
                ServiceResult result = ServiceResult.Create(runtimeResult);
                return (result.Status == ServiceStatus.Success);
            }
            service.Dispose();
            return false;
        }

        public bool Logout(string username, string password)
        {
            ServiceParameter param = new ServiceParameter().Define(p =>
            {
                p.Command = "Logout";
                p.Parameters.Add("username", username);
                p.Parameters.Add("password", password);
            });
            IWebService service = ServiceFactory.Create();
            ServiceRuntimeResult runtimeResult = service.Execute(param.ToRuntime());
            if (!runtimeResult.IsNull())
            {
                ServiceResult result = ServiceResult.Create(runtimeResult);
                return (result.Status == ServiceStatus.Success);
            }
            service.Dispose();
            return false;
        }

        public void Dispose()
        {
            if (ServiceFactory != null)
            {
                ServiceFactory.Dispose();
            }
        }
    }
}
