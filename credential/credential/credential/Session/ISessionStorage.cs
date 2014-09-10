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
using Toyota.Common.Credential;

namespace Toyota.Common.Credential
{
    public interface ISessionStorage
    {
        void Save(UserSession session);
        void Delete(UserSession session);
        void Delete(string sessionId);
        UserSession Load(UserSession session);
    }
}
