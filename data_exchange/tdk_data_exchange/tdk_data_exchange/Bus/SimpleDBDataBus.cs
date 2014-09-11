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
using Toyota.Common.Database;
using Toyota.Common.Utilities;

namespace Toyota.Common.DataExchange
{
    public class SimpleDBDataBus: DataBus
    {
        private IDBContextManager dbManager;
        private ISqlLoader sqlLoader;
        private string contextName;

        public SimpleDBDataBus(string name, IDBContextManager dbManager, string contextName): base(name)
        {
            this.dbManager = dbManager;
            this.contextName = contextName;
        }

        public override void Push(DataPacket packet)
        {
            if (packet != null)
            {
                IDBContext db = _GetDBContext();
                db.Execute("Simple_Insert", new
                {
                    Bus = GetName(),
                    Id = packet.Id,
                    Data = Convert.ToString(packet.Data)
                });
                db.Close();
            }            
        }

        public override DataPacket Pull(string id, IDictionary<string, object> parameters)
        {
            if (!string.IsNullOrEmpty(id))
            {
                IDBContext db = _GetDBContext();
                IList<SimpleDBDataModel> mPackets = db.Fetch<SimpleDBDataModel>("Simple_Select", new
                {
                    Bus = GetName(),
                    Id = id
                });
                db.Close();

                if (!mPackets.IsNullOrEmpty())
                {
                    SimpleDBDataModel model = mPackets.First();
                    DataPacket packet = new DataPacket(model.Id, GetName(), param => { })
                    {
                        Data = model.Data
                    };
                    if (!parameters.IsNullOrEmpty())
                    {
                        packet.Parameters.Merge(parameters);
                    }                    
                    return packet;
                }
            }
            return null;
        }

        public override void Close()
        {
            dbManager.RemoveSqlLoader(sqlLoader);
        }

        public override void Open()
        {
            sqlLoader = new AssemblyFileSqlLoader(GetType().Assembly, "Toyota.Common.DataExchange.SQL");
            dbManager.AddSqlLoader(sqlLoader);

            IDBContext db = _GetDBContext();
            db.Execute("Simple_CreateTable");
            db.Close();
        }

        private IDBContext _GetDBContext()
        {
            IDBContext db = dbManager.GetContext(contextName);
            db.SetExecutionMode(DBContextExecutionMode.ByName);
            return db;
        }
    }
}
