INSERT INTO [TB_M_ROLE]
           ([SYSTEM_ID]
           ,[ROLE_ID]
           ,[ROLE_NAME]
           ,[ROLE_DESCRIPTION]
           ,[AREA_ID]
           ,[SESSION_TIME_OUT]
           ,[CREATED_BY]
           ,[CREATED_DATE])
     VALUES
           (@SystemId
           ,@Id
           ,@Name
           ,@Description
           ,@AreaId
           ,@SessionTimeout
           ,@CreatedBy
           ,@CreatedDate)