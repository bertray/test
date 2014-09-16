INSERT INTO [TB_M_FEATURE_QUALIFIER]
           ([ROLE_ID]
           ,[FUNCTION_ID]
           ,[FEATURE_ID]
           ,[DATA_KEY]
           ,[QUALIFIER]
           ,[CREATED_BY]
           ,[CREATED_DATE])
     VALUES
           (@RoleId
           ,@FunctionId
           ,@FeatureId
           ,@Key
           ,@Qualifier
           ,@CreatedBy
           ,@CreatedDate)