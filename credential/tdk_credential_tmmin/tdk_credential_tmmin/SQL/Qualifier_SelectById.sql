SELECT [ROLE_ID] as "RoleId"
      ,[FUNCTION_ID] as "FunctionId"
      ,[FEATURE_ID] as "FeatureId"
      ,[DATA_KEY] as "Key"
      ,[QUALIFIER] as "Qualifier"
  FROM [TB_M_FEATURE_QUALIFIER]
  WHERE ([ROLE_ID] = @RoleId) 
		and ([FUNCTION_ID] = @FunctionId) 
		and ([FEATURE_ID] = @FeatureId)