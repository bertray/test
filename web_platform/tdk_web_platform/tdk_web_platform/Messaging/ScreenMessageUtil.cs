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
using System.Dynamic;

namespace Toyota.Common.Web.Platform
{
    class ScreenMessageUtil
    {
        private ScreenMessageUtil() { }

        public static ActionResult CreateAjaxResult(System.Web.HttpSessionStateBase session)
        {
            ScreenMessagePool messagePool = (ScreenMessagePool)session[SessionKeys.SCREEN_MESSAGE_POOL];
            return CreateAjaxResult(messagePool);
        }

        public static ActionResult CreateAjaxResult(ScreenMessagePool pool)
        {
            if (pool != null)
            {
                IList<ScreenMessage> messages = pool.Pull();
                if ((messages != null) && (messages.Count > 0))
                {
                    List<dynamic> jsList = new List<dynamic>();
                    dynamic jsMessage;
                    foreach (ScreenMessage message in messages)
                    {
                        jsMessage = new ExpandoObject();
                        jsMessage.Id = message.Name.Replace(' ', '_');
                        jsMessage.Severity = Convert.ToString(message.Severity).ToLower();
                        jsMessage.Text = message.Text;
                        jsList.Add(jsMessage);
                    }

                    return new ContentResult()
                    {
                        Content = JSON.ToString<List<dynamic>>(jsList)
                    };
                }
            }            

            return new ContentResult()
            {
                Content = JSON.ToString<IList<dynamic>>(new List<dynamic>())
            };
        }
    }
}
