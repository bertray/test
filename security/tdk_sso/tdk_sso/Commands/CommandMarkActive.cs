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
using Toyota.Common.Web.Service;
using Toyota.Common.Utilities;
using Toyota.Common.Credential;
using Toyota.Common.Database;

namespace Toyota.Common.SSO.Commands
{
    public class CommandMarkActive: ServiceCommand
    {
        public CommandMarkActive() : base("MarkActive") { }

        public override ServiceResult Execute(ServiceParameter parameter)
        {
            ServiceResult result = null;
            if (!parameter.IsNull() && parameter.Parameters.HasKey("id"))
            {
                IDBContext db = null;
                try
                {
                    string id = parameter.Parameters.Get<string>("id");
                    DateTime today = DateTime.Now;
                    db = SSO.Instance.DatabaseManager.GetContext();
                    db.Execute("Login_MarkActive", new { Id = id });                    
                    result = new ServiceResult() { Status = ServiceStatus.Success };
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
