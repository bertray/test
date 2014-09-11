using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Toyota.Common.Web.Platform
{
    public class LogSettings
    {
        public LogSettings()
        {
            InternalSinkName = "Internal-Sink";
        }

        public bool Enabled { set; get; }
        public string FolderLocation { set; get; }
        public string InternalSinkName { set; get; }
    }
}
