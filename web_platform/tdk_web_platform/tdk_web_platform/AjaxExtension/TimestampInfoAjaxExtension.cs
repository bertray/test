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
using Toyota.Common.Utilities;

namespace Toyota.Common.Web.Platform
{
    public class TimestampInfoAjaxExtension: IAjaxExtension
    {
        public string GetName()
        {
            return "Timestamp-Info";
        }

        public System.Web.Mvc.ActionResult Execute(System.Web.HttpRequestBase request, System.Web.HttpResponseBase response, System.Web.HttpSessionStateBase session)
        {
            string dateFormat = request.Params["dateFormat"];
            string timeFormat = request.Params["timeFormat"];
            if (string.IsNullOrEmpty(dateFormat))
            {
                dateFormat = "dd MMM yyyy";
            }
            if (string.IsNullOrEmpty(timeFormat))
            {
                timeFormat = "HH:mm:ss";
            }

            DateTime today = DateTime.Now;
            string currentDate = today.ToString(dateFormat);
            string currentTime = today.ToString(timeFormat);

            IDictionary<string, string> resultMap = new Dictionary<string, string>();
            resultMap.Add("Time", currentTime);
            resultMap.Add("Date", currentDate);

            return new ContentResult() {                
                Content = JSON.ToString<IDictionary<string,string>>(resultMap)
            };
        }
    }
}
