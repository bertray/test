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
        private Walker() { }

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

        public void Start()
        {
            _Timer = new Timer(Configurations.Instance.WalkerWorkingPeriod);
            _Timer.Elapsed += CheckLogins;
            _Timer.Start();            
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
                
                IList<LoginModel> logins = db.Fetch<LoginModel>("Login_Select");
                if (!logins.IsNullOrEmpty())
                {
                    DateTime today = DateTime.Now;
                    int mindiff;
                    foreach (LoginModel login in logins)
                    {
                        if (!login.LastActive.IsNull() && (login.LastActive > SqlDateTime.MinValue))
                        {
                            mindiff = today.Subtract(login.LastActive).Minutes;
                        }
                        else
                        {
                            mindiff = today.Subtract(login.LoginTime).Minutes;
                        }                        

                        if (mindiff >= login.SessionTimeout)
                        {
                            db.Execute("Login_History_Insert", new
                            {
                                Username = login.Username,
                                LoginTime = login.LoginTime,
                                LogoutTime = today,
                                SessionTimeout = login.SessionTimeout,
                                LockTimeout = login.LockTimeout
                            });

                            db.Execute("Login_Delete", new { Username = login.Username });
                        }
                        else
                        {
                            if (!login.UnlockTime.IsNull())
                            {
                                mindiff = today.Subtract(login.UnlockTime).Minutes;
                            }
                            else
                            {
                                mindiff = today.Subtract(login.LoginTime).Minutes;
                            }

                            if (mindiff >= login.LockTimeout)
                            {
                                db.Execute("Login_Lock", new
                                {
                                    Username = login.Username,
                                    LockTime = today
                                });
                            }
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