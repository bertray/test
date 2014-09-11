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
    public interface IBackgroundTaskHistory: IDisposable
    {
        IList<BackgroundTask> GetAll();
        IList<BackgroundTask> GetAll(long pageNumber, long pageSize);
        void Register(BackgroundTask task);
        IList<BackgroundTask> SearchByFunctionName(string functionName);
        IList<BackgroundTask> SearchByType(TaskType type);
        IList<BackgroundTask> SearchByName(string name);

        void Remove(params BackgroundTask[] tasks);
        void RemoveByFunctionName(string functionName);
        void RemoveByType(TaskType type);
        void RemoveByName(string name);
        void RemoveAll();
    }
}
