INSERT INTO [TB_M_SYSTEM]
           ([SYSTEM_KEY]
           ,[SYSTEM_ID]
           ,[SYSTEM_NAME]
           ,[SYSTEM_DESCRIPTION]
           ,[SYSTEM_URL]
           ,[CREATED_BY]
           ,[CREATED_DATE])
     VALUES
           (NEWID()
           ,@Id
           ,@Name
           ,@Description
           ,@Url
           ,@CreatedBy
           ,@CreatedDate)


