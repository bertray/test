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
using System.Web;
using System.Threading;
using Toyota.Common.Credential;
using Toyota.Common.Utilities;

namespace Toyota.Common.Security
{
    public class SingleSignOnSessionMonitor: IDisposable
    {
        public SingleSignOnSessionMonitor(IUserProvider userProvider, ISessionProvider sessionProvider)
        {
            UserProvider = userProvider;
            SessionProvider = sessionProvider;
            WorkingInterval = 10000;
            WorkingBlockSize = 500;

            Policies = new List<ISingleSignOnPolicy>();
            InitPolicies();
        }

        protected IUserProvider UserProvider { set; get; }
        protected ISessionProvider SessionProvider { set; get; }        
        public IList<ISingleSignOnPolicy> Policies { private set; get; }

        public int WorkingBlockSize { set; get; }
        public bool IsAlive { private set; get; }        
        public int WorkingInterval { set; get; }
        private Thread WorkingThread { set; get; }
        public bool IsThreadAlive 
        {
            get
            {
                return (WorkingThread != null) && (!WorkingThread.IsAlive);
            }
        }
        public void Start()
        {
            if(!Policies.IsNullOrEmpty() && !UserProvider.IsNull() && !SessionProvider.IsNull()) 
            {
                WorkingThread = new Thread(new ParameterizedThreadStart(DoWork));
                WorkingThread.Start();
            }            
        }

        protected virtual void DoWork(object param)
        {
            int policyCount;
            long sessionCount;
            ISingleSignOnPolicy policy;
            bool keepProcessingPolicy;
            IList<UserSession> sessions;

            IsAlive = true;
            while (IsAlive)
            {
                sessionCount = SessionProvider.GetSessionCount();
                if (sessionCount <= WorkingBlockSize)
                {
                    sessions = SessionProvider.GetSessions();
                    if (sessions.IsNull())
                    {
                        continue;
                    }
                    
                    policyCount = Policies.Count;
                    for (int i = 0; i < policyCount; i++)
                    {
                        policy = Policies[i];
                        keepProcessingPolicy = policy.Apply(sessions, SessionProvider, UserProvider);
                        if (!keepProcessingPolicy)
                        {
                            break;
                        }
                    }
                }
                Thread.Sleep(WorkingInterval);
            }
        }
        protected virtual void InitPolicies() 
        {
            Policies.Add(new LoginPolicy());
            Policies.Add(new LockPolicy());
        }

        public void Stop()
        {
            IsAlive = false;
        }
        public void Dispose()
        {
            if (!WorkingThread.IsNull() && WorkingThread.IsAlive)
            {
                WorkingThread.Abort();
            }
        }
    }
}