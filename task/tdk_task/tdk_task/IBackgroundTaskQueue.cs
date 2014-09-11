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
    public interface IBackgroundTaskQueue: IDisposable
    {
        void Register(BackgroundTask task);

        IList<BackgroundTask> GetAll();
        IList<BackgroundTask> GetAll(long pageNumber, long pageSize);
        BackgroundTask GetById(string id);
        IList<BackgroundTask> GetByFunctionName(string functionName);
        IList<BackgroundTask> GetByType(TaskType type);
        IList<BackgroundTask> GetByName(string name);

        void RemoveById(string id);
        void RemoveByFunctionName(string functionName);
        void RemoveByType(TaskType type);
        void RemoveByName(string name);
        void RemoveAll();

        void SetMaxConcurrentExecution(int numExecution);
        int GetMaxConcurrentExecution();

        IList<BackgroundTask> GetByStatus(TaskStatus status);
        void RemoveByStatus(TaskStatus status);
    }
}