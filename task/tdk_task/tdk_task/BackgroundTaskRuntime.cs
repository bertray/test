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
using System.Threading;

namespace Toyota.Common.Task
{
    public abstract class BackgroundTaskRuntime: IDisposable
    {
        private byte progress;
        private TaskStatus status;

        public BackgroundTaskRuntime()
        {
            listeners = new List<IBackgroundTaskRuntimeListener>();
            listenerLock = new Object();
        }
                
        public virtual void SetProgress(byte progress)
        {
            this.progress = progress;
            NotifyListener(new BackgroundTaskRuntimeEvent()
            {
                Runtime = this,
                Type = BackgroundTaskRuntimeEventType.Progress_Changed
            });

            if (progress > 100)
            {
                this.progress = 100;
                SetStatus(TaskStatus.Finished);
            }
            if (progress < 0)
            {
                this.progress = 0;
            }
        }
        public byte GetProgress()
        {
            return progress;
        }
        public virtual void SetStatus(TaskStatus status)
        {
            this.status = status;
            NotifyListener(new BackgroundTaskRuntimeEvent()
            {
                Runtime = this,
                Type = BackgroundTaskRuntimeEventType.Status_Changed
            });
        }
        public TaskStatus GetStatus() 
        {
            return status;
        }

        public string Id { set; get; }
        public string Name { set; get; }
        public DateTime? StartTime { set; get; }
        public DateTime? FinishTime { set; get; }
        public BackgroundTaskParameter Parameters { set; get; }

        protected void _OnExceptionOccured(Exception ex)
        {
            OnExceptionOccured(ex);
            SetStatus(TaskStatus.Aborted);
        }
        protected abstract void OnExceptionOccured(Exception ex);
        private void _OnBeforeRuntimeExecution() 
        {
            OnBeforeRuntimeExecution();
        }
        protected abstract void OnBeforeRuntimeExecution();
        private void _OnAfterRuntimeExecution() 
        {
            OnAfterRuntimeExecution();
        }
        protected abstract void OnAfterRuntimeExecution();

        protected void _PerformProcessing() 
        {            
            PerformProcessing();
        }
        protected abstract void PerformProcessing();
        public virtual void Execute(string parameter)
        {
            if (string.IsNullOrEmpty(parameter))
            {
                return;
            }

            try
            {
                Parameters = BackgroundTaskParameter.FromString(parameter);
                Id = Parameters.Get(BackgroundTask.PARAMETER_ID);
                if (string.IsNullOrEmpty(Name))
                {
                    Name = Parameters.Get(BackgroundTask.PARAMETER_NAME);
                    if (string.IsNullOrEmpty(Name))
                    {
                        Name = GetType().Name;
                    }
                }
                
                ExecutionProcess();
            }
            catch (Exception ex)
            {
                FinishTime = DateTime.Now;
                OnExceptionOccured(ex);
                throw ex;
            }
        }
        public virtual void Execute(BackgroundTaskParameter parameters) 
        {
            try
            {
                ExecutionProcess();
            }
            catch (Exception ex)
            {
                FinishTime = DateTime.Now;
                OnExceptionOccured(ex);
                throw ex;
            }
        }
        private void ExecutionProcess()
        {
            StartTime = DateTime.Now;
            SetProgress(0);
            SetStatus(TaskStatus.Active);
            _OnBeforeRuntimeExecution();
            _PerformProcessing();
            FinishTime = DateTime.Now;
            SetStatus(TaskStatus.Finished);
            _OnAfterRuntimeExecution();

            listeners.Clear();
        }

        private Object listenerLock;
        private List<IBackgroundTaskRuntimeListener> listeners;
        public void AddListener(IBackgroundTaskRuntimeListener lst)
        {
            if (!listeners.Contains(lst))
            {
                listeners.Add(lst);
            }
        }
        public void RemoveListener(IBackgroundTaskRuntimeListener lst)
        {
            listeners.Remove(lst);
        }
        public IList<IBackgroundTaskRuntimeListener> GetListeners()
        {
            if (listeners.Count > 0)
            {
                return listeners.AsReadOnly();
            }
            return null;
        }
        protected void NotifyListener(BackgroundTaskRuntimeEvent evt)
        {
            IBackgroundTaskRuntimeListener[] listenerArray = null;
            lock (listenerLock)
            {
                listenerArray = listeners.ToArray();
            }

            if (listenerArray != null)
            {
                foreach (IBackgroundTaskRuntimeListener lst in listenerArray)
                {
                    lst.BackgroundTaskRuntimeChanged(evt);
                }
            }            
        }

        public virtual void Dispose()
        {
            if (listeners.Count > 0)
            {
                listeners.Clear();
            }
        }
    }
}
