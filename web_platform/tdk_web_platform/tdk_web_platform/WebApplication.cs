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
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Toyota.Common.Configuration;
using Toyota.Common.Configuration.Binder;
using System.IO;
using Toyota.Common.Logging;
using Toyota.Common.Database;
using Toyota.Common.Database.Petapoco;
using Toyota.Common.Broadcasting;
using Toyota.Common.Document;
using Toyota.Common.Document.NPOI;
using System.Threading;
using Toyota.Common.Credential;
using Toyota.Common.Utilities;

namespace Toyota.Common.Web.Platform
{
    public abstract class WebApplication : HttpApplication
    {
        public const string DEFAULT_ROUTE_NAME = "_default_route_";
        public WebApplication() { }
        public WebApplication(string name)
        {
            ApplicationSettings.Instance.Name = name;
        }

        protected virtual void Startup() { }
        protected void Application_Start()
        {
            InitConfiguration();
            InitLocations();
            InitMessaging();
            InitLogging(); 

            LoggingSession logSession = LogManager.Instance.SystemSession;
            logSession.WriteLine();
            logSession.WriteLine(new LoggingMessage("------------------------------------------------------------"));
            logSession.WriteLine(new LoggingMessage("BOOTING UP !"));
                        
            InitDatabase();
            InitUtils();
            InitSecurity();
            InitAjaxExtension();
            InitLocalization();
            InitMenu();
            InitRuntime();
            InitDataUpload();
            InitSingleSignOn();

            logSession.WriteLine(new LoggingMessage("Executing startup sequence ..."));
            Startup();
            logSession.WriteLine(new LoggingMessage("Startup sequence complete."));
            logSession.Close();            

            logSession.Write(new LoggingMessage("Customizing routing ..."));
            RouteCollection routes = RouteTable.Routes;
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("{resource}.ashx/{*pathInfo}");
            routes.MapRoute(
                DEFAULT_ROUTE_NAME, // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = ApplicationSettings.Instance.Runtime.HomeController, action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );
            logSession.WriteLine(new LoggingMessage("OK", true));
        }

        private void InitUtils()
        {
            ProviderRegistry.Instance.Register<IHtmlMimeTypeProvider>(typeof(DefaultHtmlMimeTypeProvider));
        }
        private void InitSingleSignOn()
        {   
            string path = Path.Combine(ApplicationSettings.Instance.Deployment.HomeFolderLocation, GlobalConstants.Instance.Configuration.Session.Name);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            ApplicationSettings.Instance.Security.SSOSessionStoragePath = path;

            //IConfigurationBinder binder = new VolatileConfigurationBinder(GlobalConstants.Instance.Configuration.Session.Name);
            //binder.AddConfiguration(new ConfigurationItem() { 
            //    Key = GlobalConstants.Instance.Configuration.Session.HomeFolder,
            //    Value = path,
            //    Description = "Session home folder"
            //});
            //ApplicationConfigurationCabinet.Instance.AddBinder(binder);
        }
        private void InitDataUpload()
        {
            ProviderRegistry.Instance.Register<IExcelTabularDataFileParserFactory>(
                typeof(NPOIExcelTabularDataFileParserFactory), 
                ApplicationSettings.Instance.DataUpload.ValidationResultPath, null
            );
        }
        private void InitMessaging()
        {            
            ProviderRegistry.Instance.Register<IBroadcastingTerminal>(typeof(BroadcastingTerminal), "Application-Message-Bus");
            ApplicationMessageBus.Instance.RegisterStation(new BroadcastingStation("System"));
            AjaxExtensionRegistry.Instance.Add(new ScreenMessageAjaxExtension());
            AjaxExtensionRegistry.Instance.Add(new ScreenMessageSubmissionAjaxExtension());
        }
        private void InitDatabase()
        {
            ProviderRegistry.Instance.Register<IDBContextManager>(typeof(PetaPocoContextManager), new object[] { null, null }); 
            DatabaseManager dbManager = DatabaseManager.Instance;

            IConfigurationBinder binder = ApplicationConfigurationCabinet.Instance.GetBinder(GlobalConstants.Instance.Configuration.Database.Name);
            ConfigurationItem[] connections = binder.GetConfigurations();
            if (connections != null)
            {
                CompositeConfigurationItem config;
                ConnectionDescriptor connectionDescriptor;
                ConfigurationItem configItem;
                for (int i = 0; i < connections.Length; i++)
                {
                    config = (CompositeConfigurationItem) connections[i];

                    connectionDescriptor = new ConnectionDescriptor();
                    connectionDescriptor.Name = config.Key;

                    configItem = config.GetItem("ConnectionString");
                    if (configItem != null)
                    {
                        connectionDescriptor.ConnectionString = configItem.Value;
                    }

                    configItem = config.GetItem("IsDefault");
                    if (configItem != null)
                    {
                        connectionDescriptor.IsDefault = configItem.Value.ToLower().Equals("true") ? true : false;
                    }

                    configItem = config.GetItem("Provider");
                    if (configItem != null)
                    {
                        connectionDescriptor.ProviderName = configItem.Value;
                    }

                    dbManager.AddConnectionDescriptor(connectionDescriptor);
                }

                IList<ConnectionDescriptor> connectionDescriptors = dbManager.GetConnectionDescriptors();
                bool foundDefault = false;
                foreach (ConnectionDescriptor descriptor in connectionDescriptors)
                {
                    if (descriptor.IsDefault)
                    {
                        foundDefault = true;
                    }
                }

                if (!foundDefault)
                {
                    if (connectionDescriptors.Count > 0)
                    {
                        dbManager.SetDefaultConnectionDescriptor(connectionDescriptors[0]);
                    }
                }
            }
            else
            {
                CompositeConfigurationItem sampleItem = new CompositeConfigurationItem()
                {
                    Key = "DummyDatabaseName",
                    Value = "ConnectionString: | Default:true | Provider: System.Data.SqlClient",
                    Description = "Dummy Database"
                };
                sampleItem.AddItem(new ConfigurationItem() { Key = "ConnectionString", Value = "Your-Connection-String" });
                sampleItem.AddItem(new ConfigurationItem() { Key = "IsDefault", Value = "true" });
                sampleItem.AddItem(new ConfigurationItem() { Key = "Provider", Value = "System.Data.SqlClient" });
                binder.AddConfiguration(sampleItem);
                binder.Save();
            }
                        
            dbManager.AddSqlLoader(new FileSqlLoader(GlobalConstants.Instance.Location.SQLStatement));      
        }
        private void InitLogging()
        {
            LogSettings loggingSettings = ApplicationSettings.Instance.Logging;
            if (loggingSettings.Enabled)
            {
                string path = ApplicationSettings.Instance.Deployment.HomeFolderLocation + "\\Logs";
                loggingSettings.FolderLocation = path;
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }                
            }
        }
        private void InitLocations()
        {            
            string path = ApplicationSettings.Instance.Deployment.HomeFolderLocation;
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }            

            path = GlobalConstants.Instance.Location.Configuration;
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            path = GlobalConstants.Instance.Location.SQLStatement;
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            /* Data Upload */
            //IConfigurationBinder binder = ApplicationConfigurationCabinet.Instance.GetBinder(GlobalConstants.Instance.Configuration.DataUpload.Name);
            //path = ApplicationSettings.Instance.Deployment.HomeFolderLocation + "\\" + binder.GetConfiguration(GlobalConstants.Instance.Configuration.DataUpload.RootFolder).Value;
            //if (!Directory.Exists(path))
            //{
            //    Directory.CreateDirectory(path);
            //}

            //path = ApplicationSettings.Instance.Deployment.HomeFolderLocation + "\\" +
            //    binder.GetConfiguration(GlobalConstants.Instance.Configuration.DataUpload.RootFolder).Value + "\\" +
            //    binder.GetConfiguration(GlobalConstants.Instance.Configuration.DataUpload.ValidationResultFolder).Value;
            //if (!Directory.Exists(path))
            //{
            //    Directory.CreateDirectory(path);
            //}            
        }
        private void InitMenu()
        {
            if (ApplicationSettings.Instance.Menu.Enabled)
            {
                ProviderRegistry.Instance.Register<IMenuProvider>(typeof(XmlFileMenuProvider));
                IMenuProvider menuProvider = ProviderRegistry.Instance.Get<IMenuProvider>();
            }
        }
        private void InitLocalization()
        {
            ProviderRegistry.Instance.Register<ILocalizedWordCollection>(typeof(TextFileLocalizedCollection), new object[] { 
                GlobalConstants.Instance.Location.Configuration, GlobalConstants.Instance.Configuration.Locale.Name 
            });
            ILocalizedWordCollection locale = ProviderRegistry.Instance.Get<ILocalizedWordCollection>();
            IConfigurationBinder binder = ApplicationConfigurationCabinet.Instance.GetBinder(GlobalConstants.Instance.Configuration.System.Name);
            if (binder != null)
            {
                ConfigurationItem config = binder.GetConfiguration(GlobalConstants.Instance.Configuration.Locale.Default);
                if (config != null)
                {
                    locale.SetDefaultCode(config.Value);
                }
            }            
        }
        private void InitAjaxExtension()
        {
            AjaxExtensionRegistry.Instance.Add(new InputValidationAjaxExtension());
            AjaxExtensionRegistry.Instance.Add(new TimestampInfoAjaxExtension());
            AjaxExtensionRegistry.Instance.Add(new GlobalSearchAjaxExtension());
        }        
        private void InitConfiguration()
        {
            ApplicationSettings appSettings = ApplicationSettings.Instance;
            ApplicationConfigurationCabinet cabinet = ApplicationConfigurationCabinet.Instance;
            string path = GlobalConstants.Instance.Location.Configuration;

            IConfigurationBinder binder = new XmlFileConfigurationBinder(GlobalConstants.Instance.Configuration.System.Name, path);
            cabinet.AddBinder(binder);
            binder.Load();

            ConfigurationItem config = binder.GetConfiguration(GlobalConstants.Instance.Configuration.System.DeploymentContext);
            if (config == null)
            {
                config = new ConfigurationItem()
                {
                    Key = GlobalConstants.Instance.Configuration.System.DeploymentContext,
                    Value = "",
                    Description = "Relative path for deployment"
                };
                binder.AddConfiguration(config);                
            }
            appSettings.Deployment.Context.Name = config.Value;

            config = binder.GetConfiguration(GlobalConstants.Instance.Configuration.System.DevelopmentStage);
            if (config == null)
            {
                config = new ConfigurationItem()
                {
                    Key = GlobalConstants.Instance.Configuration.System.DevelopmentStage,
                    Value = DevelopmentSettings.STAGE_DEVELOPMENT.Code,
                    Description = "Development stage"
                };
                binder.AddConfiguration(config);
            }
            string stageName = config.Value.ToUpper();
            if (stageName.Equals(DevelopmentSettings.STAGE_DEVELOPMENT.Code))
            {
                appSettings.Development.Stage = DevelopmentSettings.STAGE_DEVELOPMENT;
            }
            else if (stageName.Equals(DevelopmentSettings.STAGE_PRODUCTION))
            {
                appSettings.Development.Stage = DevelopmentSettings.STAGE_PRODUCTION;
            }
            else if (stageName.Equals(DevelopmentSettings.STAGE_QUALITY_ASSURANCE))
            {
                appSettings.Development.Stage = DevelopmentSettings.STAGE_QUALITY_ASSURANCE;
            }
            else
            {
                appSettings.Development.Stage = new DevelopmentStage() { Code = stageName };
            }

            config = binder.GetConfiguration(GlobalConstants.Instance.Configuration.Locale.Default);
            if (config == null)
            {
                config = new ConfigurationItem()
                {
                    Key = GlobalConstants.Instance.Configuration.Locale.Default,
                    Value = "US",
                    Description = "Default localization"
                };
                binder.AddConfiguration(config);
            }

            config = binder.GetConfiguration(GlobalConstants.Instance.Configuration.System.HomeFolder);
            if (config == null)
            {
                config = new ConfigurationItem()
                {
                    Key = GlobalConstants.Instance.Configuration.System.HomeFolder,
                    Value = "C:\\Application_Home\\" + ApplicationSettings.Instance.Alias.Replace(' ', '_'),
                    Description = "Home folder path"
                };
                binder.AddConfiguration(config);
            }
            appSettings.Deployment.HomeFolderLocation = config.Value;

            binder.Save(); // Save system binder

            binder = new DifferentialXmlConfigurationBinder(GlobalConstants.Instance.Configuration.Database.Name, appSettings.Development.Stage.Code, path);
            cabinet.AddBinder(binder);
            binder.Load();
            binder.Save();

            binder = new DifferentialXmlConfigurationBinder(GlobalConstants.Instance.CONFIGURATION_SINGLE_SIGN_ON_BINDER, appSettings.Development.Stage.Code, path);
            cabinet.AddBinder(binder);
            binder.Load();
            binder.Save();

            binder = new AssemblyTextFileConfigurationBinder("Encryption", "Toyota.Common.Web.Platform.Configurations", GetType().Assembly);
            cabinet.AddBinder(binder);
            binder.Load();
            binder.Save();

            binder = ApplicationConfigurationCabinet.Instance.GetBinder(GlobalConstants.Instance.CONFIGURATION_SINGLE_SIGN_ON_BINDER);
            binder.Load();
            config = binder.GetConfiguration(GlobalConstants.Instance.CONFIGURATION_SINGLE_SIGN_ON_URL);
            if (config == null)
            {
                config = new ConfigurationItem()
                {
                    Key = GlobalConstants.Instance.CONFIGURATION_SINGLE_SIGN_ON_URL,
                    Value = "url-to-single-sign-on-service",
                    Description = "Url of Single Sign On Service"
                };
                binder.AddConfiguration(config);
            }
            ApplicationSettings.Instance.Security.SSOServiceUrl = config.Value;
            binder.Save();

            /*
             * Volatile configurations
             */ 
            binder = new VolatileConfigurationBinder(GlobalConstants.Instance.Configuration.DataUpload.Name);
            binder.AddConfiguration(new ConfigurationItem()
            {
                Key = GlobalConstants.Instance.Configuration.DataUpload.RootFolder,
                Value = "DataUpload",
                Description = "Data upload home folder"
            });
            binder.AddConfiguration(new ConfigurationItem()
            {
                Key = GlobalConstants.Instance.Configuration.DataUpload.ValidationResultFolder,
                Value = "Validation_Result",
                Description = "Data upload validation result folder"
            });
            cabinet.AddBinder(binder);

            binder = new VolatileConfigurationBinder(GlobalConstants.Instance.Configuration.Session.Name);
            binder.AddConfiguration(new ConfigurationItem()
            {
                Key = GlobalConstants.Instance.Configuration.Session.HomeFolder,
                Value = "Sessions",
                Description = "Persisted session storage"
            });
        }
        private void InitRuntime()
        {
            Patrol patrol = Patrol.Instance;

            ApplicationSettings.Instance.Runtime.Browser.BlockedAgents.Add(BrowserAgent.Chrome("2"));
            ApplicationSettings.Instance.Runtime.Browser.BlockedAgents.Add(BrowserAgent.Firefox("21"));
            ApplicationSettings.Instance.Runtime.Browser.BlockedAgents.Add(BrowserAgent.InternetExplorer("7"));
            ApplicationSettings.Instance.Runtime.Browser.BlockedAgents.Add(BrowserAgent.InternetExplorer("6"));
            ApplicationSettings.Instance.Runtime.Browser.BlockedAgents.Add(BrowserAgent.InternetExplorer("5"));
            ApplicationSettings.Instance.Runtime.Browser.BlockedAgents.Add(BrowserAgent.InternetExplorer("4"));
            ApplicationSettings.Instance.Runtime.Browser.BlockedAgents.Add(BrowserAgent.InternetExplorer("3"));
            ApplicationSettings.Instance.Runtime.Browser.BlockedAgents.Add(BrowserAgent.InternetExplorer("2"));
            ApplicationSettings.Instance.Runtime.Browser.BlockedAgents.Add(BrowserAgent.InternetExplorer("1"));
        }
        private void InitSecurity()
        {
            //ProviderRegistry.Instance.Register<IEncryptionAgent>(typeof(AES256EncryptionAgent));
            if (ApplicationSettings.Instance.Security.EnableSingleSignOn)
            {
                AjaxExtensionRegistry.Instance.Add(new SingleSignOnAjaxExtension());
            }
        }

        //protected void ActivateSingleSignOn()
        //{
        //    ApplicationSettings.Instance.Security.EnableSingleSignOn = true;
        //    //AjaxExtensionRegistry.Instance.Add(new SingleSignOnAjaxExtension());
        //}
    }
}
