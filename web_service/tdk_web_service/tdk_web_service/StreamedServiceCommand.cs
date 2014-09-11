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
using Toyota.Common.Utilities;
using System.IO;

namespace Toyota.Common.Web.Service
{
    public abstract class StreamedServiceCommand: IServiceCommand
    {
        public StreamedServiceCommand(string name)
        {
            this.name = name;
        }

        public abstract StreamedServiceResult Execute(StreamedServiceParameter parameter);

        private string name;
        public string Name
        {
            get { return name; }
        }
    }
}
