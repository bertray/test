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
    public class AjaxExtensionRegistry
    {
        private static readonly AjaxExtensionRegistry instance = new AjaxExtensionRegistry();
        private IDictionary<string, IAjaxExtension> extensions;

        private AjaxExtensionRegistry() 
        {
            extensions = new Dictionary<string, IAjaxExtension>();
        }

        public static AjaxExtensionRegistry Instance
        {
            get
            {
                return instance;
            }
        }

        public void Add(IAjaxExtension extension)
        {
            string name = extension.GetName();
            if (string.IsNullOrEmpty(name))
            {
                return;
            }

            if (extensions.ContainsKey(name))
            {
                extensions[name] = extension;
            }
            else
            {
                extensions.Add(name, extension);
            }
        }

        public void Remove(string name)
        {
            if (extensions.ContainsKey(name))
            {
                extensions.Remove(name);
            }
        }

        public void Remove(IAjaxExtension extension)
        {
            Remove(extension.GetName());
        }

        public bool HasExtension(string name)
        {
            return extensions.ContainsKey(name);
        }

        public IAjaxExtension Get(string name)
        {
            if (extensions.ContainsKey(name))
            {
                return extensions[name];
            }
            return null;
        }
    }
}
