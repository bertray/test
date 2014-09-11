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

namespace Toyota.Common.Task
{
    public interface IBackgroundTaskExecutor
    {
        BackgroundTaskRuntime Execute(BackgroundTask task);
        int GetRunningTaskCount();
        void SetMaximumRunningTask(int count);
        int GetMaximumRunningTask();

        void AddListener(IBackgroundTaskExecutorListener listener);
        void RemoveListener(IBackgroundTaskExecutorListener listener);
        IList<IBackgroundTaskExecutorListener> GetListeners();
    }
}
