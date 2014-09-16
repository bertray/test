INSERT INTO [TB_M_ROLE_FEATURE]
           ([ROLE_ID]
           ,[FUNCTION_ID]
           ,[FEATURE_ID]
           ,[CREATED_BY]
           ,[CREATED_DATE])
     VALUES
           (@RoleId
           ,@FunctionId
           ,@FeatureId
           ,@CreatedBy
           ,@CreatedDate)