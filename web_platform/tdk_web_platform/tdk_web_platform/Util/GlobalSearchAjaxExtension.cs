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
using System.Web.Mvc;

namespace Toyota.Common.Web.Platform
{
    public class GlobalSearchAjaxExtension: IAjaxExtension
    {
        public string GetName()
        {
            return "Global-Search";
        }

        public System.Web.Mvc.ActionResult Execute(System.Web.HttpRequestBase request, System.Web.HttpResponseBase response, System.Web.HttpSessionStateBase session)
        {
            string key = request.Params["key"];
            IList<GlobalSearchResult> results = new List<GlobalSearchResult>();
            if (!string.IsNullOrEmpty(key))
            {
                IGlobalSearch search = ProviderRegistry.Instance.Get<IGlobalSearch>();
                if (search != null)
                {
                    results = search.Search(key);
                }
            }
            return new ContentResult() {
                Content = JSON.ToString<IList<GlobalSearchResult>>(results)
            };
        }
    }
}
