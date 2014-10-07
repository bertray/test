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
using Toyota.Common.Credential;

namespace Toyota.Common.Web.Platform
{
    public class SSOClient: IDisposable
    {
        private SSOClient() 
        {
            ServiceFactory = new ServiceClientFactory<IWebService>(ApplicationSettings.Instance.Security.SSOServiceUrl);
        }

        private static SSOClient instance = null;
        public static SSOClient Instance
        {
            get
            {
                if (instance.IsNull())
                {
                    instance = new SSOClient();
                }
                return instance;
            }
        }

        private ServiceClientFactory<IWebService> ServiceFactory { set; get; }        

        public string Login(string username, string password, string hostname, 
            string hostIP, string browser, string browserVersion, bool isMobile)
        {
            string id = null;
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
                if (!result.IsNull() && (result.Status == ServiceStatus.Success) && !result.Data.IsNull())
                {
                    id = result.Data.Get<string>("Id");
                }
            }
            service.Dispose();
            return id;
        }

        public bool IsUserLocked(string id)
        {
            bool resultState = false;
            ServiceParameter param = new ServiceParameter().Define(p =>
            {
                p.Command = "IsUserLocked";
                p.Parameters.Add("id", id);
            });
            IWebService service = ServiceFactory.Create();
            ServiceRuntimeResult runtimeResult = service.Execute(param.ToRuntime());
            if (!runtimeResult.IsNull())
            {
                ServiceResult result = ServiceResult.Create(runtimeResult);
                if (!result.IsNull() && (result.Status == ServiceStatus.Success) && !result.Data.IsNull())
                {
                    resultState = result.Data.Get<bool>("IsUserLocked");
                }
            }
            service.Dispose();
            return resultState;
        }

        public bool Lock(string id)
        {
            bool resultState = false;
            ServiceParameter param = new ServiceParameter().Define(p =>
            {
                p.Command = "IsUserLocked";
                p.Parameters.Add("id", id);
            });
            IWebService service = ServiceFactory.Create();
            ServiceRuntimeResult runtimeResult = service.Execute(param.ToRuntime());
            if (!runtimeResult.IsNull())
            {
                ServiceResult result = ServiceResult.Create(runtimeResult);
                resultState = !result.IsNull() && (result.Status == ServiceStatus.Success);
            }
            service.Dispose();
            return resultState;
        }

        public bool Unlock(string id)
        {
            bool resultState = false;
            ServiceParameter param = new ServiceParameter().Define(p =>
            {
                p.Command = "Unlock";
                p.Parameters.Add("id", id);
            });
            IWebService service = ServiceFactory.Create();
            ServiceRuntimeResult runtimeResult = service.Execute(param.ToRuntime());
            if (!runtimeResult.IsNull())
            {
                ServiceResult result = ServiceResult.Create(runtimeResult);
                resultState = !result.IsNull() && (result.Status == ServiceStatus.Success);
            }
            service.Dispose();
            return resultState;
        }

        public bool Logout(string id)
        {
            bool resultState = false;
            ServiceParameter param = new ServiceParameter().Define(p =>
            {
                p.Command = "Logout";
                p.Parameters.Add("id", id);
            });
            IWebService service = ServiceFactory.Create();
            ServiceRuntimeResult runtimeResult = service.Execute(param.ToRuntime());
            if (!runtimeResult.IsNull())
            {
                ServiceResult result = ServiceResult.Create(runtimeResult);
                resultState = !result.IsNull() && (result.Status == ServiceStatus.Success);
            }
            service.Dispose();
            return resultState;
        }

        public bool IsSessionAlive(string id)
        {
            ServiceParameter param = new ServiceParameter().Define(p =>
            {
                p.Command = "IsSessionAlive";
                p.Parameters.Add("Id", id);
            });
            IWebService service = ServiceFactory.Create();
            ServiceRuntimeResult runtimeResult = service.Execute(param.ToRuntime());
            if (!runtimeResult.IsNull())
            {
                ServiceResult result = ServiceResult.Create(runtimeResult);
                return !result.IsNull() && (result.Status == ServiceStatus.Confirmed);
            }
            service.Dispose();
            return false;
        }

        public string GetLoggedInUser(string id)
        {
            ServiceParameter param = new ServiceParameter().Define(p =>
            {
                p.Command = "GetLoggedInUser";
                p.Parameters.Add("Id", id);
            });
            IWebService service = ServiceFactory.Create();
            ServiceRuntimeResult runtimeResult = service.Execute(param.ToRuntime());
            if (!runtimeResult.IsNull())
            {
                ServiceResult result = ServiceResult.Create(runtimeResult);
                if (!result.IsNull() && (result.Status == ServiceStatus.Success) && !result.Data.IsNull())
                {
                    return result.Data.Get<string>("GetLoggedInUser");
                }
            }
            service.Dispose();
            return null;
        }

        public bool MarkActive(string id)
        {
            bool resultState = false;
            ServiceParameter param = new ServiceParameter().Define(p =>
            {
                p.Command = "MarkActive";
                p.Parameters.Add("id", id);
            });
            IWebService service = ServiceFactory.Create();
            ServiceRuntimeResult runtimeResult = service.Execute(param.ToRuntime());
            if (!runtimeResult.IsNull())
            {
                ServiceResult result = ServiceResult.Create(runtimeResult);
                resultState = !result.IsNull() && (result.Status == ServiceStatus.Success);
            }
            service.Dispose();
            return resultState;
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
