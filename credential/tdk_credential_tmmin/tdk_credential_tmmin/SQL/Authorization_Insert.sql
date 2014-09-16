INSERT INTO [TB_M_USER_ASSIGNMENT]
           ([USERNAME]
           ,[DIVISION_CODE]
           ,[ROLE_ID]
           ,[SYSTEM_ID]
           ,[CREATED_BY]
           ,[CREATED_DATE])
     VALUES
           (@Username
           ,@DivisionCode
           ,@RoleId
           ,@SystemId
           ,@CreatedBy
           ,@CreatedDate)