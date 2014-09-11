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
using System.Data;

namespace Toyota.Common.Database.Petapoco
{
    public class PetaPocoDBContext : BaseDBContext
    {        
        protected PetaPoco.Database db;    

        public PetaPocoDBContext(IDbConnection connection, ConnectionDescriptor connectionDescriptor, IList<ISqlLoader> sqlLoaders): base(connectionDescriptor, sqlLoaders)
        {
            if (connection != null)
            {                
                db = new PetaPoco.Database(connection);
            }
            else
            {
                db = new PetaPoco.Database(connectionDescriptor.ConnectionString, connectionDescriptor.ProviderName);
            }            
            db.EnableAutoSelect = false;
            db.EnableNamedParams = true;
            db.ForceDateTimesToUtc = false;
            db.OpenSharedConnection();
        }
        public PetaPocoDBContext(ConnectionDescriptor connectionDescriptor, IList<ISqlLoader> sqlLoaders)
            : this(null, connectionDescriptor, sqlLoaders)
        {            
        }
               
        public override void BeginTransaction()
        {
            db.BeginTransaction();
        }
        public override void AbortTransaction()
        {
            db.AbortTransaction();
        }
        public override void CommitTransaction()
        {
            db.CompleteTransaction();
        }

        public override IPagedData<T> FetchByPage<T>(string sqlContext, long pageNumber, long pageSize, object[] args)
        {
            sqlContext = LoadSqlContext(sqlContext);
            PetaPoco.Page<T> page = db.Page<T>(pageNumber, pageSize, sqlContext, args != null ? args : new object[] {});
            if (page != null)
            {
                return new PetaPocoPagedData<T>(page);
            }

            return null;
        }
        public override IEnumerable<T> Query<T>(string sqlContext, params object[] args)
        {
            sqlContext = LoadSqlContext(sqlContext);
            return db.Query<T>(sqlContext, args != null ? args : new object[] { });
        }
        public override IList<T> Fetch<T>(string sqlContext, params object[] args)
        {
            sqlContext = LoadSqlContext(sqlContext);
            return db.Fetch<T>(sqlContext, args != null ? args : new object[] { });
        }
        public override T SingleOrDefault<T>(string sqlContext, params object[] args)
        {
            sqlContext = LoadSqlContext(sqlContext);
            return db.SingleOrDefault<T>(sqlContext, args != null ? args : new object[] { });
        }
        public override T ExecuteScalar<T>(string sqlContext, params object[] args)
        {
            sqlContext = LoadSqlContext(sqlContext);
            return db.ExecuteScalar<T>(sqlContext, args != null ? args : new object[] { });
        }
        public override int Execute(string sqlContext, params object[] args)
        {
            sqlContext = LoadSqlContext(sqlContext);
            return db.Execute(sqlContext, args != null ? args : new object[] { });
        }
        
        public override void Close() 
        {
            if (db != null)
            {
                db.CloseSharedConnection();
            }
        }
    }
}
