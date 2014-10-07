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
using Toyota.Common.Utilities;
using Toyota.Common.Database;
using Toyota.Common.Credential;

namespace Toyota.Common.SSO
{
    internal class CommandUnlock: ServiceCommand
    {
        public CommandUnlock() : base("Unlock") { }

        public override ServiceResult Execute(ServiceParameter parameter)
        {
            ServiceResult result = null;
            if (!parameter.IsNull() && parameter.Parameters.HasKey("id"))
            {
                IDBContext db = null;
                try
                {
                    result = new ServiceResult();
                    string id = parameter.Parameters.Get<string>("id");
                    result = new ServiceResult();
                    DateTime today = DateTime.Now;
                    db = SSO.Instance.DatabaseManager.GetContext();
                    db.Execute("Login_Unlock", new { Id = id, UnlockTime = today });
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