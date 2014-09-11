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

namespace Toyota.Common.Logging.Sink
{
    public class DatabaseLoggingSink: LoggingSink
    {
        private IDBContextManager dbManager;
        private ISqlLoader sqlLoader;

        private string insertStatement;
        private string selectStatement;
        private string selectLastIndexStatement;
        private string updateStatement;        

        public DatabaseLoggingSink(string name, IDBContextManager dbManager) : this(name, "TB_R_LOG", null, dbManager) { }
        public DatabaseLoggingSink(string name, string tableName, IDBContextManager dbManager) : this(name, tableName, null, dbManager) { }
        public DatabaseLoggingSink(string name, string tableName, string contextName, IDBContextManager dbManager): base(name)
        {
            this.dbManager = dbManager;
            sqlLoader = new AssemblyFileSqlLoader(GetType().Assembly, "Toyota.Common.Logging.SQL");
            this.dbManager.AddSqlLoader(sqlLoader);
            DatabaseContextName = contextName;
            TableName = tableName;

            IDBContext db = CreateDBContext();
            db.SetExecutionMode(DBContextExecutionMode.Direct);
            string sql = db.LoadSqlScript("Common_Logging_DBSink_Create");
            sql = sql.Replace("@TableName", tableName);
            db.Execute(sql);

            insertStatement = db.LoadSqlScript("Common_Logging_DBSink_Save");
            insertStatement = insertStatement.Replace("@TableName", tableName);
            selectStatement = db.LoadSqlScript("Common_Logging_DBSink_Select");
            selectStatement = selectStatement.Replace("@TableName", tableName);
            selectLastIndexStatement = db.LoadSqlScript("Common_Logging_DBSink_SelectLastIndex");
            selectLastIndexStatement = selectLastIndexStatement.Replace("@TableName", tableName);
            updateStatement = db.LoadSqlScript("Common_Logging_DBSink_Update");
            updateStatement = updateStatement.Replace("@TableName", tableName);

            db.Close();
        }

        public override void Write(params LoggingMessage[] messages)
        {
            if (string.IsNullOrEmpty(SessionName))
            {
                return;
            }
            if ((messages == null) || (messages.Length == 0))
            {
                return;
            }

            IDBContext db = CreateDBContext();
            LoggingMessage message;
            Nullable<Int64> lastIndex = null;
            string text;
            bool createNewEntry = false;
            for(int i = 0; i < messages.Length; i++) {
                message = messages[i];
                createNewEntry = true;

                text = message.Message;
                if (!text.Equals("\n"))
                {
                    lastIndex = db.SingleOrDefault<Nullable<Int64>>(selectLastIndexStatement, new { SessionName = SessionName });
                    if (lastIndex.HasValue)
                    {
                        createNewEntry = false;
                    }
                }
                else
                {
                    text = string.Empty;
                }

                if (!createNewEntry)
                {
                    db.Execute(updateStatement, new
                    {
                        Message = text,
                        SessionName = SessionName,
                        Id = lastIndex
                    });
                }
                else
                {
                    db.Execute(insertStatement, new
                    {
                        Session = SessionName,
                        Severity = message.Severity,
                        Date = message.Date,
                        Message = text
                    });
                }
            }            

            db.Close();
        }
        public override void Close()
        {
            this.dbManager.RemoveSqlLoader(sqlLoader);
        }

        public override IList<LoggingMessage> Pull()
        {
            IDBContext db = CreateDBContext();
            IList<LoggingMessage> messages = db.Fetch<LoggingMessage>(selectStatement, new { SessionName = SessionName });
            db.Close();

            return messages;
        }
        public override IList<LoggingMessage> Pull(int pageIndex, int pageSize)
        {
            IDBContext db = CreateDBContext();
            IPagedData<LoggingMessage> pagedMessages = db.FetchByPage<LoggingMessage>(selectStatement, pageIndex, pageSize, new { SessionName = SessionName });
            db.Close();

            return pagedMessages.GetData();
        }

        public string TableName { private set; get; }
        public string DatabaseContextName { private set; get; }

        private IDBContext CreateDBContext()
        {
            IDBContext db = null;
            if (string.IsNullOrEmpty(DatabaseContextName))
            {
                db = dbManager.GetContext();
            }
            else
            {
                db = dbManager.GetContext(DatabaseContextName);
            }
            db.SetExecutionMode(DBContextExecutionMode.ByName);

            return db;
        }
    }
}
