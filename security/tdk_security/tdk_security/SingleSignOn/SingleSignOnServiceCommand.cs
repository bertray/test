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
    public class SingleSignOnServiceCommand
    {
        private SingleSignOnServiceCommand() { }

        public const string GetUsers = "_usrs";
        public const string GetUser = "_usr";
        public const string GetUserByName = "_usr.nm";
        public const string GetUserByFirstName = "_usr.ftnm";
        public const string GetUserByLastName = "_usr.lsnm";
        public const string GetUserCount = "_usr.cnt"; 
        public const string AuthenticateUser = "_usr.auth";
        public const string FetchAuthorization = "_usr.ftc.auth";
        public const string FetchOrganization = "_usr.ftc.org";
        public const string FetchPlant = "_usr.ftc.plt";
        public const string Login = "_login";
        public const string Logout = "_logout";
        public const string Lock = "_lock";
        public const string Unlock = "_unlock";
        public const string GetSessions = "_ses.all";
        public const string GetSession = "_ses";
        public const string RemoveSession = "_ses.del";
        public const string UpdateSession = "_ses.upd";
        public const string SaveSessionHistory = "_ses.hist.save";
        public const string GetSessionHistories = "_ses.hists";
    }
}
