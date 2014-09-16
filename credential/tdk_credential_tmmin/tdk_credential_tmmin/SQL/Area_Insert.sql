INSERT INTO [TB_M_AREA]
           ([AREA_KEY]
           ,[AREA_ID]
           ,[AREA_NAME]
           ,[CREATED_BY]
           ,[CREATED_DATE])
     VALUES
           (NEWID()
           ,@Id
           ,@Name
           ,@CreatedBy
           ,@CreatedDate)