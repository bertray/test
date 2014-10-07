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
using System.Timers;
using Toyota.Common.Database;
using Toyota.Common.Utilities;
using Toyota.Common.Web.Service;
using System.Data.SqlTypes;

namespace Toyota.Common.SSO
{
    internal class Walker: IDisposable
    {
        private Walker() 
        {
            Policy = new DefaultPolicy();
        }

        private static Walker instance = null;
        public static Walker Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Walker();
                }
                return instance;
            }
        }

        private Timer _Timer { set; get; }
        public ISSOPolicy Policy { set; get; }

        public void Start()
        {
            if (!Policy.IsNull())
            {
                _Timer = new Timer(Configurations.Instance.WalkerWorkingPeriod);
                _Timer.Elapsed += CheckLogins;
                _Timer.Start();            
            }            
        }

        public void Stop()
        {
            if (_Timer != null)
            {
                _Timer.Stop();
            }
        }

        public void Dispose()
        {
            if (_Timer != null)
            {
                _Timer.Dispose();
            }
        }

        private void CheckLogins(object source, ElapsedEventArgs args)
        {
            IDBContext db = SSO.Instance.DatabaseManager.GetContext();
            try
            {
                db.BeginTransaction();
                DateTime today = DateTime.Now;
                IList<SSOLoginInfo> infos = db.Fetch<SSOLoginInfo>("Login_Select");
                if (!infos.IsNullOrEmpty())
                {
                    SSOPolicyState state;
                    foreach (SSOLoginInfo info in infos)
                    {
                        state = Policy.Evaluate(info, today);
                        switch (state)
                        {
                            case SSOPolicyState.SessionExpired:
                                db.Execute("Login_History_Insert", new
                                {
                                    Id = info.Id,
                                    Username = info.Username,
                                    LoginTime = today,
                                    SessionTimeout = info.SessionTimeout,
                                    LockTimeout = info.LockTimeout,
                                    MaxLogin = info.MaximumLogin,
                                    Hostname = info.Hostname,
                                    HostIP = info.HostIP,
                                    Browser = info.Browser,
                                    BrowserVersion = info.BrowserVersion,
                                    IsMobile = info.IsMobile
                                });
                                db.Execute("Login_Delete", new { Id = info.Id });                                
                                break;
                            case SSOPolicyState.LockActive:
                                db.Execute("Login_Lock", new { Id = info.Id, LockTime = today });
                                break;
                            default: break;
                        }
                    }
                }
                db.CommitTransaction();
            }
            catch (Exception ex)
            {
                db.AbortTransaction();
                throw ex;
            }
            finally
            {
                db.Close();
            }
        }
    }
}