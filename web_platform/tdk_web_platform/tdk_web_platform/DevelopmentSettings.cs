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
    public class DevelopmentSettings
    {
        public static readonly DevelopmentStage STAGE_DEVELOPMENT = new DevelopmentStage() { Code = "DEV", Description = "Development" };
        public static readonly DevelopmentStage STAGE_QUALITY_ASSURANCE = new DevelopmentStage() { Code = "QA", Description = "Quality Assurance" };
        public static readonly DevelopmentStage STAGE_PRODUCTION = new DevelopmentStage() { Code = "PROD", Description = "Production" };

        public DevelopmentStage Stage { set; get; }
    }
}
