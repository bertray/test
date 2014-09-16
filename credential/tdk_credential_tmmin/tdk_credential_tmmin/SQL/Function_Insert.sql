INSERT INTO [TB_M_FUNCTION]
           ([FUNCTION_KEY]
           ,[FUNCTION_ID]
           ,[SYSTEM_ID]
           ,[MODULE_ID]
           ,[FUNCTION_NAME]
           ,[FUNCTION_DESCRIPTION]
           ,[CREATED_BY]
           ,[CREATED_DATE])
     VALUES
           (NEWID()
           ,@Id
           ,@SystemId
           ,@ModuleId
           ,@Name
           ,@Description
           ,@CreatedBy
           ,@CreatedDate)