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
using System.Collections.Specialized;
using Toyota.Common.Configuration;
using Toyota.Common.Utilities;
using System.IO;

namespace Toyota.Common.Web.Platform
{
    [RequestFilter]
    public abstract class PageController: Controller
    {
        public const string DEFAULT_ACTION = "Index";
        protected byte PAGE_STATE_INIT = 0;

        public PageController()
        {
            Settings = new PageSettings(GetType());
            Descriptor = new PageDescriptor();
            Authentication = new SessionAuthentication(Settings.ScreenID, Descriptor);
            IgnoreStartup = false;
            PageState = PAGE_STATE_INIT;
        }

        public PageSettings Settings { private set; get; }
        public SessionAuthentication Authentication { private set; get; }
        public PageDescriptor Descriptor { private set; get; }
        public ILookup Lookup
        {
            get
            {
                return (ILookup) Session[SessionKeys.LOOKUP];
            }
        }
        public ILocalizedWordCollection Locale
        {
            get
            {
                return ObjectPool.Instance.Factory.GetInstance<ILocalizedWordCollection>();
            }
        }
        public ScreenMessagePool ScreenMessages
        {
            get
            {
                return (ScreenMessagePool) Session[SessionKeys.SCREEN_MESSAGE_POOL];
            }
        }
        public object Model { set; get; }
        public bool IgnoreStartup { set; get; }
        public byte PageState { set; get; }
                
        protected override void Initialize(RequestContext requestContext)
        {
            HttpSessionStateBase session = requestContext.HttpContext.Session;
            if (session.IsNewSession)
            {
                ILookup lookup = new SimpleLookup();
                
                session[SessionKeys.LOOKUP] = lookup;
                session[SessionKeys.SCREEN_MESSAGE_POOL] = new ScreenMessagePool();
            }
            ILookup lkp = (ILookup) session[SessionKeys.LOOKUP];
            Descriptor.Initialize(requestContext);
            ForwardPageParameters(ViewData, requestContext.HttpContext.Request);
            base.Initialize(requestContext);
        }
        public ActionResult Index()
        {               
            if (Authentication.IsValid)
            {
                User user = Lookup.Get<User>();
                ViewData["User"] = user;
                ViewData["_tdkAuthenticatedUser"] = user;
                if (!IgnoreStartup)
                {
                    Startup();
                }                
                Settings.AttachToRequest(ViewData);
                Descriptor.AttachToRequest(ViewData);

                if (ApplicationSettings.Instance.Menu.Enabled)
                {
                    IMenuProvider menuProvider = ObjectPool.Instance.Factory.GetInstance<IMenuProvider>();
                    if (menuProvider != null)
                    {
                        if (!menuProvider.IsBaseUrlAttached)
                        {
                            menuProvider.AttachBaseUrl(Descriptor.BaseUrl);
                        }
                        MenuList menus = menuProvider.GetAll();
                        ViewData["Menu"] = menus;
                        ViewData["_tdkApplicationMenu"] = menus;
                    }
                }
            }
            if (!ScreenMessages.IsNull())
            {
                ViewData["_tdkScreenMessages"] = ScreenMessages.Pull();
            }

            if (!Authentication.RedirectUrl.IsNullOrEmpty())
            {
                return Redirect(Authentication.RedirectUrl);
            }
            if (Request.Browser.IsMobileDevice && ApplicationSettings.Instance.Runtime.EnableMobileSupport)
            {
                return View(string.Format("m{0}", Settings.IndexPage));
            }
            return View(Settings.IndexPage, Model);
        }
        public ActionResult IndexIgnoreStartup()
        {
            IgnoreStartup = true;
            return Index();
        }
        protected void ResetIndexPage()
        {
            string className = GetType().Name;
            Settings.IndexPage = className.Substring(0, className.IndexOf("Controller"));
        }

        [HttpPost]
        public ActionResult AjExt(string name)
        {
            AjaxExtensionRegistry extensionPool = AjaxExtensionRegistry.Instance;
            if (!string.IsNullOrEmpty(name) && extensionPool.HasExtension(name))
            {
                IAjaxExtension extension = extensionPool.Get(name);
                return extension.Execute(Request, Response, Session);
            }

            return new ContentResult() { Content = string.Empty };
        }

        protected virtual void Startup() { }
        private void ForwardPageParameters(ViewDataDictionary viewData, HttpRequestBase request)
        {
            NameValueCollection param = request.Params;
            IDictionary<string, string> forwardParams = new Dictionary<string, string>();
            foreach (string key in param.Keys)
            {
                if (!string.IsNullOrEmpty(key) && key.StartsWith("_p"))
                {
                    forwardParams.Add(key.Substring(2, key.Length - 2), param[key]); 
                }
            }
            viewData["_tdkForwardedParameters"] = forwardParams;
        }
        protected string GetParameter(string key)
        {
            string value = Request.Params[key];
            if (value.IsNullOrEmpty() && ViewData.ContainsKey("_tdkForwardedParameters"))
            {
                IDictionary<string, string> forwardParams = (IDictionary<string, string>)ViewData["_tdkForwardedParameters"];
                if (forwardParams.ContainsKey(key))
                {
                    value = forwardParams[key];
                }
            }
            return value;
        }

        public FileResult PullFile(string section, string name)
        {
            //Image img = Image.FromFile(string.Format("{0}/{1}/{2}", ApplicationSettings.Instance.Deployment.HomeFolderLocation, section, name));
            //MemoryStream stream = new MemoryStream();
            //img.Save(stream, img.RawFormat);
            
            //return File(stream, mimeType);
            string path = Path.Combine(ApplicationSettings.Instance.Deployment.HomeFolderLocation, section, name);
            string mimeType = ProviderRegistry.Instance.Get<IHtmlMimeTypeProvider>().GetMimeType(Path.GetExtension(name));
            FileStream fstream = new FileStream(path, FileMode.Open);
            return new FileStreamResult(fstream, mimeType);
        }
        public FileResult PullImage(string section, string name)
        {
            return PullFile(section + Path.DirectorySeparatorChar + "Images", name);
        }
        public FileResult PullUserPicture(string name)
        {
            return PullImage("Users", name.Replace('.','_') + ".png");
        }
    }
}
