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
using System.Web.Mvc;
using System.Web.Routing;
using System.Web;
using Toyota.Common.Lookup;
using Toyota.Common.Credential;

namespace Toyota.Common.Web.Platform
{
    public class RequestFilterAttribute: ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            PageController controller = (PageController)filterContext.Controller;
            string controllerName = controller.GetType().Name.ToLower();
            HttpSessionStateBase session = filterContext.HttpContext.Session;

            ActionResult actionResult = ApplyBrowserRestriction(filterContext.HttpContext.Request);
            if (actionResult != null)
            {
                filterContext.Result = actionResult;
                return;
            }

            actionResult = EvaluateRuntimeMode(controllerName);
            if (actionResult != null)
            {
                filterContext.Result = actionResult;
                return;
            }                       

            controller.Authentication.Authenticate(filterContext.RequestContext);
            if (!controller.Authentication.IsValid)
            {                
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(
                    new { 
                        Controller = ApplicationSettings.Instance.Security.LoginController, 
                        Action = PageController.DEFAULT_ACTION
                    }
                ));
            }
            else if (!controller.Authentication.IsAuthorized && !ApplicationSettings.Instance.Security.IgnoreAuthorization) {
                if (string.IsNullOrEmpty(ApplicationSettings.Instance.Security.UnauthorizedController))
                {
                    filterContext.Result = new ContentResult() { Content = new MvcHtmlString("You are not authorized to access this page.").ToHtmlString() };
                }
                else
                {
                    filterContext.Result = new RedirectResult("/" + ApplicationSettings.Instance.Security.UnauthorizedController);
                }                
            }
        }

        private ActionResult ApplyBrowserRestriction(HttpRequestBase request)
        {
            HttpBrowserCapabilitiesBase browser = request.Browser;
            BrowserSettings browserSettings = ApplicationSettings.Instance.Runtime.Browser;
            if (browserSettings.EnableRestriction && (browserSettings.BlockedAgents.Count > 0))
            {
                foreach (BrowserAgent agent in browserSettings.BlockedAgents)
                {
                    if (agent.IsAgentEquals(browser))
                    {
                        if (string.IsNullOrEmpty(browserSettings.BlockingControllerName))
                        {
                            StringBuilder stringBuilder = new StringBuilder();
                            stringBuilder.AppendLine("<!DOCTYPE html>                                       ");
                            stringBuilder.AppendLine("<html>                                                ");
                            stringBuilder.AppendLine("  <head>                                              ");
                            stringBuilder.AppendLine("      <style type=\"text/css\">                       ");
                            stringBuilder.AppendLine("          #message                                    ");
                            stringBuilder.AppendLine("          {                                           ");
                            stringBuilder.AppendLine("              padding: 20px;                          ");
                            stringBuilder.AppendLine("              margin-top: 30px;                       ");
                            stringBuilder.AppendLine("              margin-left: auto;                      ");
                            stringBuilder.AppendLine("              margin-right: auto;                     ");
                            stringBuilder.AppendLine("              width: 50%;                             ");
                            stringBuilder.AppendLine("              color: #525252;                         ");
                            stringBuilder.AppendLine("              border: 1px solid #525252;              ");
                            stringBuilder.AppendLine("              border-radius: 20px;                    ");
                            stringBuilder.AppendLine("              -moz-border-radius: 20px;               ");
                            stringBuilder.AppendLine("              -webkit-border-radius: 20px;            ");
                            stringBuilder.AppendLine("          }                                           ");
                            stringBuilder.AppendLine("      </style>                                        ");
                            stringBuilder.AppendLine("  </head>                                             ");
                            stringBuilder.AppendLine("  <body>                                              ");
                            stringBuilder.AppendLine("      <div id=\"message\">                            ");
                            stringBuilder.AppendLine("          Your browser is not supported by this application. Unsupported browsers are follow: ");
                            stringBuilder.AppendLine("          <ul>                                        ");
                            foreach (BrowserAgent browserAgent in browserSettings.BlockedAgents)
                            {
                                stringBuilder.AppendLine(string.Format("              <li>{0} {1}</li>", browserAgent.Name, browserAgent.Version));
                            }
                            stringBuilder.AppendLine("          </ul>");
                            stringBuilder.AppendLine("      </div>");
                            stringBuilder.AppendLine("  </body>");
                            stringBuilder.AppendLine("</html>");

                            return new ContentResult() { Content = stringBuilder.ToString() };
                        }
                        else
                        {
                            return new RedirectToRouteResult(new RouteValueDictionary(
                                new
                                {
                                    Controller = browserSettings.BlockingControllerName,
                                    Action = PageController.DEFAULT_ACTION
                                }
                            ));
                        }
                    }
                }
            }

            return null;
        }
        private ActionResult EvaluateRuntimeMode(string controllerName)
        {
            StringBuilder template = new StringBuilder();
            template.AppendLine("<!DOCTYPE html>                                       ");
            template.AppendLine("<html>                                                ");
            template.AppendLine("  <head>                                              ");
            template.AppendLine("      <style type=\"text/css\">                       ");
            template.AppendLine("          #message                                    ");
            template.AppendLine("          {                                           ");
            template.AppendLine("              padding: 20px;                          ");
            template.AppendLine("              margin-top: 100px;                       ");
            template.AppendLine("              margin-left: auto;                      ");
            template.AppendLine("              margin-right: auto;                     ");
            template.AppendLine("              width: 50%;                             ");
            template.AppendLine("              color: #525252;                         ");
            template.AppendLine("              text-align: center;                     ");
            template.AppendLine("              border: 1px solid #525252;              ");
            template.AppendLine("              border-radius: 20px;                    ");
            template.AppendLine("              -moz-border-radius: 20px;               ");
            template.AppendLine("              -webkit-border-radius: 20px;            ");
            template.AppendLine("          }                                           ");
            template.AppendLine("      </style>                                        ");
            template.AppendLine("  </head>                                             ");
            template.AppendLine("  <body>                                              ");
            template.AppendLine("      <div id=\"message\">{0}</div>                   ");
            template.AppendLine("  </body>");
            template.AppendLine("</html>");

            RuntimeSettings runtimeSettings = ApplicationSettings.Instance.Runtime;
            if (runtimeSettings.Mode == RuntimeMode.Offline)
            {
                if (string.IsNullOrEmpty(runtimeSettings.OfflineModeController))
                {
                    return new ContentResult() { Content = template.Replace("{0}", "The application is currently shutted down, please contact the administrator for further information.").ToString() };
                }
                else if (!controllerName.Equals(runtimeSettings.OfflineModeController.ToLower() + "controller"))
                {
                    return new RedirectToRouteResult(new RouteValueDictionary(
                        new
                        {
                            Controller = runtimeSettings.OfflineModeController,
                            Action = PageController.DEFAULT_ACTION
                        }
                    ));
                }
            }
            else if (runtimeSettings.Mode == RuntimeMode.Maintenance)
            {
                if (string.IsNullOrEmpty(runtimeSettings.MaintenanceModeController))
                {
                    return new ContentResult() { Content = template.Replace("{0}", "The application is currently under maintenance, please come back a while later.").ToString() };
                }
                else
                {
                    if (!controllerName.Equals(runtimeSettings.MaintenanceModeController.ToLower() + "controller")) 
                    {
                        return new RedirectToRouteResult(new RouteValueDictionary(
                            new
                            {
                                Controller = runtimeSettings.MaintenanceModeController,
                                Action = PageController.DEFAULT_ACTION
                            }
                        ));
                    }                    
                }
            }
            return null;
        }
        private string GetBaseUrl(HttpRequestBase request)
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
                return string.Format("{0}://{1}/{2}", scheme, request.ServerVariables["HTTP_HOST"], deploymentContext);
            }
            else
            {
                return string.Format("{0}://{1}", scheme, request.ServerVariables["HTTP_HOST"]);
            }
        }
    }
}
