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
    public interface IMenuProvider
    {
        Menu Get(string id);
        MenuList GetByUrl(string url);
        MenuList GetByText(string text);
        MenuList GetAll();
        void AttachBaseUrl(string baseUrl);
        bool IsBaseUrlAttached { get; }

        void Save();
        void Load();
    }
}
