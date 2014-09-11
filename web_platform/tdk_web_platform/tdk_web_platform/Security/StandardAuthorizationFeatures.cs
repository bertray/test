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
    public sealed class StandardAuthorizationFeatures
    {
        private StandardAuthorizationFeatures() { }

        public const string Addition = "TDK-ADD";
        public const string Browsing = "TDK-BROWSE";
        public const string Chatting = "TDK-CHAT";
        public const string Creation = "TDK-CREATE";
        public const string Deletion = "TDK-DELETE";
        public const string Download = "TDK-DOWNLOAD";
        public const string Modification = "TDK-MODIFY";
        public const string Removal = "TDK-REMOVE";
        public const string Messaging = "TDK-SEND-MESSAGING";
        public const string Upload = "TDK-UPLOAD";
        public const string Viewing = "TDK-VIEW";
        public const string DataExchange = "TDK-DATA-EXCHANGE";
    }
}
