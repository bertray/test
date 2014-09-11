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

namespace Toyota.Common.Web.Platform
{
    public class ApplicationSettings
    {
        private ApplicationSettings()
        {
            Deployment = new DeploymentSettings();
            Security = new SecuritySettings();
            Development = new DevelopmentSettings();
            Menu = new MenuSettings();
            Logging = new LogSettings();
            Runtime = new RuntimeSettings();
            DataUpload = new DataUploadSettings();
            UI = new UserInterfaceSettings();

            Name = "Application Name";
            Alias = "AppAlias";
            OwnerName = "Application Owner";
            OwnerAlias = "App-Own";
            OwnerEmail = "owner@theapplication.com";
        }

        private static readonly ApplicationSettings instance = new ApplicationSettings();
        public static ApplicationSettings Instance
        {
            get
            {                
                return instance;
            }
        }
                
        public DeploymentSettings Deployment { private set; get; }
        public DevelopmentSettings Development { private set; get; }
        public SecuritySettings Security { private set; get; }
        public MenuSettings Menu { private set; get; }
        public LogSettings Logging { private set; get; }
        public RuntimeSettings Runtime { private set; get; }
        public DataUploadSettings DataUpload { private set; get; }
        public UserInterfaceSettings UI { private set; get; }

        public string Name { set; get; }
        public string Alias { set; get; }
        public string OwnerName { set; get; }
        public string OwnerAlias { set; get; }
        public string OwnerEmail { set; get; }
    }
}
