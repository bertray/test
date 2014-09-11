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
using Toyota.Common.Task;

namespace Toyota.Common.Web.Platform
{
    public class TaskManager
    {
        private TaskManager() {}

        public static BackgroundTaskManager Instance
        {
            get
            {
                return ProviderRegistry.Instance.Get<BackgroundTaskManager>();
            }
        }
    }
}
