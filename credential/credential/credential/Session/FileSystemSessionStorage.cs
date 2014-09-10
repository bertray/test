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
using Toyota.Common.Utilities;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Toyota.Common.Lookup;

namespace Toyota.Common.Credential
{
    public class FileSystemSessionStorage: ISessionStorage
    {        
        public FileSystemSessionStorage(string basePath)
        {
            BasePath = basePath;
            Extension = "session";
        }

        public string BasePath { set; get; }
        public string Extension { set; get; }

        public void Save(UserSession session)
        {
            if (!session.IsNull())
            {
                string path = string.Format("{0}/{1}.{2}", BasePath, session.Id, Extension);
                if (File.Exists(path))
                {
                    File.Delete(path);
                }

                FileStream file = File.Create(path, 512);
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(file, session.Data);
                file.Close();
            }            
        }

        public UserSession Load(UserSession session)
        {
            if (!session.IsNull())
            {
                string path = string.Format("{0}/{1}.{2}", BasePath, session.Id, Extension);
                if (File.Exists(path))
                {
                    FileStream file = File.OpenRead(path);
                    BinaryFormatter formatter = new BinaryFormatter();
                    session.Data = (ILookup) formatter.Deserialize(file);
                    file.Close();   
                }                
            }

            return session;
        }

        public void Delete(UserSession session)
        {
            if (!session.IsNull())
            {
                Delete(session.Id);
            }
        }

        public void Delete(string sessionId)
        {
            if (!string.IsNullOrEmpty(sessionId))
            {
                string path = string.Format("{0}/{1}.{2}", BasePath, sessionId, Extension);
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
            }
        }
    }
}
