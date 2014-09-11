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

namespace Toyota.Common.Security
{
    public class SingleSignOnServiceParameter
    {
        private SingleSignOnServiceParameter() { }

        public const string Id = "_id";
        public const string Username = "_usrn";
        public const string Password = "_pwd";
        public const string User = "_usr";
        public const string Users = "_usrs";
        public const string State = "_st";
        public const string PageNumber = "_pgNum";
        public const string PageSize = "_pgSz";
        public const string UserCount = "_cnt";
        public const string Name = "_nm";
        public const string Location = "_lct";
        public const string Client = "_cln";
        public const string Session = "_ses";
        public const string Sessions = "_ses.all";
        public const string SessionHistories = "_ses.hist.all";
    }
}
