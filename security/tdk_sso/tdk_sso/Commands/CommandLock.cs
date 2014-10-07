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
using Toyota.Common.Web.Service;
using Toyota.Common.Database;
using Toyota.Common.Utilities;
using Toyota.Common.Credential;

namespace Toyota.Common.SSO
{
    internal class CommandLock: ServiceCommand
    {
        public CommandLock() : base("Lock") { }

        public override ServiceResult Execute(ServiceParameter parameter)
        {
            ServiceResult result = null;
            IDBContext db = null;
            if (!parameter.IsNull() && parameter.Parameters.HasKey("id"))
            {
                try
                {
                    result = new ServiceResult();
                    string id = parameter.Parameters.Get<string>("id");
                    DateTime today = DateTime.Now;
                    db = SSO.Instance.DatabaseManager.GetContext();
                    db.Execute("Login_Lock", new { Id = id, LockTime = today });
                    db.Close();
                    result.Status = ServiceStatus.Success;
                }
                finally
                {
                    if (!db.IsNull())
                    {
                        db.Close();
                    }
                }                
            }
            return result;
        }
    }
}