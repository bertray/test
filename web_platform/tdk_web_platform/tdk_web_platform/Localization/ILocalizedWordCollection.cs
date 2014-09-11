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
    public interface ILocalizedWordCollection
    {
        void SetDefaultCode(string code);
        string GetDefaultCode();

        string Translate(string key);
        IDictionary<string, string> GetAll();
        IList<string> GetKeys();
    }
}
