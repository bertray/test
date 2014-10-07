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
using Toyota.Common.Database;
using System.Web.Hosting;
using System.IO;
using Toyota.Common.Database.Petapoco;
using Toyota.Common.Credential;

namespace Toyota.Common.SSO
{
    public class SSO: IDisposable
    {
        public const string Database_Context_User = "User";
        public const string Database_Context_Service = "Service";

        public SSO()
        {            
            DatabaseManager = new PetaPocoContextManager(new ISqlLoader[] {
                new AssemblyFileSqlLoader(GetType().Assembly, "Toyota.Common.SSO.SQL")
            }, new ConnectionDescriptor[] {
                new ConnectionDescriptor(Database_Context_User, Configurations.Instance.UserDBConnectionString),
                new ConnectionDescriptor(Database_Context_Service, Configurations.Instance.ServiceDBConnectionString) { IsDefault = true }
            });
            DatabaseManager.SetContextExecutionMode(DBContextExecutionMode.ByName);
        }

        private static SSO instance;
        public static SSO Instance
        {
            get
            {
                if (instance == null) 
                {
                    instance = new SSO();
                }
                return instance;
            }
        }

        public IDBContextManager DatabaseManager { set; get; }
        public IUserProvider UserProvider { set; get; }

        public void Dispose()
        {
            if (DatabaseManager != null)
            {
                DatabaseManager.Close();
            }            
        }
    }
}
