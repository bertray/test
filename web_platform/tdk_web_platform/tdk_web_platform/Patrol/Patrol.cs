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

namespace Toyota.Common.Web.Platform
{
    public class Patrol: IDisposable
    {
        private static Patrol instance = new Patrol();
        private IDictionary<string, IPatrolTask> taskMap;
        private Thread patrolThread;        
        private bool keepAlive;

        private Patrol() 
        {
            taskMap = new Dictionary<string, IPatrolTask>();
            keepAlive = true;
            patrolThread = new Thread(_ActivatePatrol);
            patrolThread.Start();
            SleepPeriod = 900000;
        }

        public static Patrol Instance
        {
            get
            {
                return instance;
            }
        }

        public int SleepPeriod { private set; get; }

        public void RegisterTask(IPatrolTask task)
        {
            if(task != null) 
            {
                string name = task.GetName();
               if(taskMap.ContainsKey(name)) 
               {
                   taskMap.Add(name, task);
               }
            }            
        }

        public void RemoveTask(string name)
        {
            if (taskMap.ContainsKey(name))
            {
                taskMap.Remove(name);
            }
        }

        private void _ActivatePatrol()
        {                    
            while(keepAlive) 
            {
                Thread.Sleep(SleepPeriod);

                GC.Collect();
                foreach (IPatrolTask task in taskMap.Values)
                {
                    if ((task.GetInterval() % SleepPeriod) == 0)
                    {
                        task.Execute(new PatrolParameter() { });   
                    }
                }
            }            
        }

        public void Dispose()
        {
            if (patrolThread != null)
            {
                keepAlive = false;
                patrolThread.Abort();
            }
        }
    }
}
