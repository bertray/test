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

namespace Toyota.Common.Web.Platform
{
    public class ScreenMessageSubmissionAjaxExtension: IAjaxExtension
    {
        public string GetName()
        {
            return "Screen-Message-Submission";
        }

        public System.Web.Mvc.ActionResult Execute(System.Web.HttpRequestBase request, System.Web.HttpResponseBase response, System.Web.HttpSessionStateBase session)
        {
            ScreenMessagePool messagePool = (ScreenMessagePool)session[SessionKeys.SCREEN_MESSAGE_POOL];
            if (messagePool != null)
            {
                string jsonMessages = request.Params["Messages"];
                if (!string.IsNullOrEmpty(jsonMessages))
                {
                    IList<ScreenMessage> messages = JSON.ToObject<IList<ScreenMessage>>(jsonMessages);
                    if (!messages.IsNullOrEmpty())
                    {
                        messagePool.Submit(messages.ToArray());
                    }
                }
            }            
            
            return ScreenMessageUtil.CreateAjaxResult(session);
        }
    }
}
