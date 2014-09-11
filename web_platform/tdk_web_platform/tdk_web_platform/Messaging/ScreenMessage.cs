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
    public class ScreenMessage
    {
        public ScreenMessage() : this(string.Empty, string.Empty, ScreenMessageSeverity.Info) { }
        public ScreenMessage(ScreenMessageSeverity severity, string text) : this(Guid.NewGuid().ToString(), text, severity) { }
        public ScreenMessage(string name) : this(name, string.Empty, ScreenMessageSeverity.Info) { }
        public ScreenMessage(string name, string text): this(name, text, ScreenMessageSeverity.Info) { }
        public ScreenMessage(string name, string text, ScreenMessageSeverity severity)
        {
            Name = name;
            Text = text;
            Severity = severity;
        }

        public string Name { set; get; }
        public string Text { set; get; }
        public ScreenMessageSeverity Severity { set; get; }

        public static ScreenMessage Success(string text)
        {
            return new ScreenMessage(Guid.NewGuid().ToString(), text, ScreenMessageSeverity.Success);
        }
        public static ScreenMessage Info(string text)
        {
            return new ScreenMessage(Guid.NewGuid().ToString(), text, ScreenMessageSeverity.Info);
        }
        public static ScreenMessage Warning(string text)
        {
            return new ScreenMessage(Guid.NewGuid().ToString(), text, ScreenMessageSeverity.Warning);
        }
        public static ScreenMessage Error(string text)
        {
            return new ScreenMessage(Guid.NewGuid().ToString(), text, ScreenMessageSeverity.Error);
        }
    }
}
