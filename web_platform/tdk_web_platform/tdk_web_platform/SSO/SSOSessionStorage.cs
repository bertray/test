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
        private SSOSessionStorage() { }

        private static SSOSessionStorage instance = null;
        public static SSOSessionStorage Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new SSOSessionStorage();
                }
                return instance;
            }
        }

        public void Save(string id, ILookup lookup)
        {
            if (!lookup.IsNull())
            {
                User user = lookup.Get<User>();
                if (!user.IsNull())
                {
                    string path = Path.Combine(ApplicationSettings.Instance.Security.SSOSessionStoragePath, id + ".sso");
                    if (File.Exists(path))
                    {
                        File.Delete(path);
                    }

                    IList<object> objects = lookup.GetAll();
                    ILookup plookup = new SimpleLookup();
                    foreach (object obj in objects)
                    {
                        plookup.Add(obj);
                    }
                    plookup.Remove<User>();

                    SessionPersistenceModel model = new SessionPersistenceModel();
                    model.Username = user.Username;
                    model.Password = user.Password;
                    model.Data = plookup;

                    FileStream file = File.Create(path, 512);
                    BinaryFormatter formatter = new BinaryFormatter();
                    formatter.Serialize(file, model);
                    file.Close();
                }
            }
        }

        public ILookup Load(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                string path = Path.Combine(ApplicationSettings.Instance.Security.SSOSessionStoragePath, id + ".sso");
                if (File.Exists(path))
                {
                    
                    FileStream file = File.OpenRead(path);
                    BinaryFormatter formatter = new BinaryFormatter();
                    SessionPersistenceModel model = (SessionPersistenceModel)formatter.Deserialize(file);
                    file.Close();
                    if (model != null)
                    {
                        IUserProvider userProvider = ProviderRegistry.Instance.Get<IUserProvider>();
                        User user = userProvider.GetUser(model.Username);
                        ILookup lookup = model.Data;
                        lookup.Add(user);

                        return lookup;
                    }
                }    
            }
            return null;
        }

        public void Delete(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                string path = Path.Combine(ApplicationSettings.Instance.Security.SSOSessionStoragePath, id + ".sso");
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
            }
        }
    }
}
