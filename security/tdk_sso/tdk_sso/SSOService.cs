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
using System.IO;
using Toyota.Common.Configuration.Binder;
using Toyota.Common.Configuration;
using System.Web.Hosting;
using Toyota.Common.Utilities;
using Toyota.Common.Credential;
using Toyota.Common.Database;

namespace Toyota.Common.SSO
{
    public class SSOService: WebService
    {
        public SSOService()
        {
            _InitConfiguration();

            SSO providers = SSO.Instance;
            Walker.Instance.Start();

            Commands.AddCommand(new CommandLogin());
            Commands.AddCommand(new CommandLogout());
            Commands.AddCommand(new CommandUnlock());
            Commands.AddCommand(new CommandLock());
            Commands.AddCommand(new CommandIsLocked());
            Commands.AddCommand(new CommandIsAlive());
            Commands.AddCommand(new CommandGetLoggedInUser());
            Commands.AddCommand(new CommandMarkActive());
            Commands.AddCommand(new CommandIsLoggedIn());
        }

        private void _InitConfiguration()
        {
            string workingPath = HostingEnvironment.ApplicationPhysicalPath;
            string configPath = Path.Combine(workingPath, "Configuration");
            if (!Directory.Exists(configPath))
            {
                Directory.CreateDirectory(configPath);
            }

            ConfigurationBinder binder = new XmlFileConfigurationBinder("system", configPath);
            binder.Load();
            ConfigurationItem config = binder.GetConfiguration("DevelopmentPhase");
            if (config.IsNull())
            {
                config = new ConfigurationItem();
                config.Key = "DevelopmentPhase";
                config.Value = "DEV";
                config.Description = "Development Phase";
                binder.AddConfiguration(config);
            }
            Configurations.Instance.DevelopmentPhase = config.Value;
            binder.Save();

            binder = new DifferentialXmlConfigurationBinder("service", Configurations.Instance.DevelopmentPhase, configPath);
            binder.Load();
            config = binder.GetConfiguration(SSO.Database_Context_User);
            if (config.IsNull())
            {
                config = new ConfigurationItem();
                config.Key = SSO.Database_Context_User;
                config.Value = "DB Connection String";
                config.Description = "User Provider's connection string";
                binder.AddConfiguration(config);
            }
            Configurations.Instance.UserDBConnectionString = config.Value;

            config = binder.GetConfiguration(SSO.Database_Context_Service);
            if (config.IsNull())
            {
                config = new ConfigurationItem();
                config.Key = SSO.Database_Context_Service;
                config.Value = "DB Connection String";
                config.Description = "SSO's connection string";
                binder.AddConfiguration(config);
            }
            Configurations.Instance.ServiceDBConnectionString = config.Value;

            config = binder.GetConfiguration("WalkerWorkingPeriod");
            if (config.IsNull())
            {
                config = new ConfigurationItem();
                config.Key = "WalkerWorkingPeriod";
                config.Value = "30000";
                config.Description = "Walker's working period";
                binder.AddConfiguration(config);
            }
            Configurations.Instance.WalkerWorkingPeriod = Convert.ToInt32(config.Value);
            binder.Save();
        }

        protected void Init(IUserProvider userProvider, ISSOPolicy policy)
        {
            SSO.Instance.UserProvider = userProvider;
            if (!policy.IsNull())
            {
                Walker.Instance.Policy = policy;
            }            
        }

        protected void Init(IUserProvider userProvider)
        {
            Init(userProvider, null);
        }
    }
}
