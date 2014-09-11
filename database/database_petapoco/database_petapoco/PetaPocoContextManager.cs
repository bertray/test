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
using System.Collections.Concurrent;
using System.Configuration;

namespace Toyota.Common.Database.Petapoco
{
    public class PetaPocoContextManager : BaseContextManager
    {
        public PetaPocoContextManager(ISqlLoader[] sqlLoaders, ConnectionDescriptor[] connectionDescriptors): base(sqlLoaders, connectionDescriptors) { }        

        public override IDBContext GetContext(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                ConnectionDescriptor descriptor = GetConnectionDescriptor(name);
                if (descriptor != null)
                {
                    PetaPocoDBContext db = new PetaPocoDBContext(descriptor, GetSqlLoaders());
                    db.SetExecutionMode(GetContextExecutionMode());
                    return db;
                }   
            }            
            return GetContext();            
        }

        public override IDBContext GetContext()
        {
            ConnectionDescriptor descriptor = GetDefaultConnectionDescriptor();
            if (descriptor != null)
            {
                return GetContext(descriptor.Name);
            }

            return null;
        }

        public override void Close()
        {            
        }
    }
}
