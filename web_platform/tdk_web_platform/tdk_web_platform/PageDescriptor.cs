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
using System.Web;
using System.Web.Routing;
using System.Web.Mvc;

namespace Toyota.Common.Web.Platform
{
    public class PageDescriptor
    {
        private HttpRequestBase request;

        public void Initialize(RequestContext requestContext)
        {
            HttpContextBase httpContext = requestContext.HttpContext;
            request = httpContext.Request;
        }

        private static string _baseUrl;
        public string BaseUrl
        {
            get
            {
                if (string.IsNullOrEmpty(_baseUrl))
                {
                    string scheme = request.Url.Scheme;
                    DeploymentContextSettings contextSettings = ApplicationSettings.Instance.Deployment.Context;
                    if (contextSettings.EmulateSecureProtocol)
                    {
                        scheme = "https";
                    }

                    string deploymentContext = contextSettings.Name;
                    if (!string.IsNullOrEmpty(deploymentContext))
                    {
                        _baseUrl = string.Format("{0}://{1}/{2}", scheme, request.ServerVariables["HTTP_HOST"], deploymentContext);
                    }
                    else
                    {
                        _baseUrl = string.Format("{0}://{1}", scheme, request.ServerVariables["HTTP_HOST"]);
                    }
                }
                return _baseUrl;
            }
        }

        public void AttachToRequest(ViewDataDictionary viewData)
        {
            viewData["_tdkBaseUrl"] = BaseUrl;
        }
    }
}
