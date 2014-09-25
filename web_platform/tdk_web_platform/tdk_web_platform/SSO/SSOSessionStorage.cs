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
using Toyota.Common.Lookup;
using Toyota.Common.Utilities;
using Toyota.Common.Credential;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Web;

namespace Toyota.Common.Web.Platform
{
    public class SSOSessionStorage
    {        
        public void Save(HttpSessionStateBase session)
        {
            ILookup lookup = session.Lookup();
            if (!lookup.IsNull())
            {
                User user = lookup.Get<User>();
                if (!user.IsNull())
                {
                    string path = Path.Combine(ApplicationSettings.Instance.Security.SSOSessionStoragePath, user.Username + ".sso");
                    if (File.Exists(path))
                    {
                        File.Delete(path);
                    }

                    FileStream file = File.Create(path, 512);
                    BinaryFormatter formatter = new BinaryFormatter();
                    formatter.Serialize(file, lookup);
                    file.Close();
                }
            }
        }

        public ILookup Load(string username)
        {
            if (!string.IsNullOrEmpty(username))
            {
                string path = Path.Combine(ApplicationSettings.Instance.Security.SSOSessionStoragePath, username + ".sso");
                if (File.Exists(path))
                {
                    
                    FileStream file = File.OpenRead(path);
                    BinaryFormatter formatter = new BinaryFormatter();
                    //session.Data = (ILookup)formatter.Deserialize(file);
                    file.Close();
                }    
            }
            return null;
        }

        public void Delete(string username)
        {
            if (!string.IsNullOrEmpty(username))
            {
                string path = Path.Combine(ApplicationSettings.Instance.Security.SSOSessionStoragePath, username + ".sso");
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
            }
        }
    }
}
